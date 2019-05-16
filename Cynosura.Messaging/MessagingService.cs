using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Messaging.Abstractions;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Options;

namespace Cynosura.Messaging
{
    public class MessagingService : IMessagingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMessagingServiceOptions _options;
        private IBusControl _connection;
        private X509Certificate _certificate;

        private readonly List<ConsumerRegistration> _registrations;
        private Action<IRabbitMqSslConfigurator, IRabbitMqHostConfigurator> _sslConfigurator;

        public MessagingService(
            IServiceProvider serviceProvider,
            IOptions<RabbitMessagingServiceOptions> options
        )
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
            _registrations = new List<ConsumerRegistration>();
        }

        private void DefaultHostConfig(IRabbitMqHostConfigurator host)
        {
            host.Username(_options.Username);
            host.Password(_options.Password);
            if (_certificate != null || _sslConfigurator != null)
            {
                host.UseSsl(ssl =>
                {
                    ssl.UseCertificateAsAuthenticationIdentity = false;
                    if (_certificate != null)
                    {
                        ssl.Certificate = _certificate;
                    }
                    ssl.Protocol = SslProtocols.Tls12;
                    if (!string.IsNullOrEmpty(_options.BrokerX509Cn))
                    {
                        ssl.ServerName = _options.BrokerX509Cn;
                    }
                    _sslConfigurator?.Invoke(ssl, host);
                });
            }
        }

        private IRabbitMqHost DefaultConfig(IRabbitMqBusFactoryConfigurator config,
            Action<IRabbitMqHostConfigurator> advanced = null)
        {
            return config.Host(new Uri(_options.ConnectionUrl), host =>
            {
                DefaultHostConfig(host);
                advanced?.Invoke(host);
            });
        }

        public void Connect(Action<IRabbitMqHostConfigurator> advanced = null)
        {
            _connection = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = DefaultConfig(config, advanced);

                foreach (var registration in _registrations)
                {
                    config.ReceiveEndpoint(host, registration.Queue,
                        ep =>
                        {
                            registration.Custom?.Invoke(ep);

                            if (registration.Consumer != null)
                            {
                                ep.Consumer(registration.ConsumerType, type => registration.Consumer);
                            }
                            if(registration.ConsumerType !=null)
                            {
                                var method = typeof(ExtensionsDependencyInjectionIntegrationExtensions)
                                    .GetTypeInfo()
                                    .GetDeclaredMethods("Consumer")
                                    .Single();
                                method.MakeGenericMethod(registration.ConsumerType)
                                    .Invoke(null, new object[] {ep, _serviceProvider, null});
                            }
                        });
                    registration.BusCustom?.Invoke(config, host);
                }
            });
        }

        public void Start()
        {
            _connection.Start();
        }

        public void Stop(TimeSpan stopTimeout)
        {
            _connection.Stop(stopTimeout);
        }

        private string GetAddress(string queue) =>
            $"{_options.ConnectionUrl}/{queue}";

        private Task Subscribe(string queue,
            IConsumer consumer,
            Type consumerType,
            Action<IRabbitMqReceiveEndpointConfigurator> custom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (consumerType == null && busCustom == null)
            {
                throw new ArgumentNullException(nameof(consumerType));
            }
            _registrations.Add(new ConsumerRegistration(queue, consumer, consumerType, custom, busCustom));
            return Task.CompletedTask;
        }

        private async Task<ISendEndpoint> GetEndpoint(string queue)
        {
            var endpoint = await _connection.GetSendEndpoint(new Uri(GetAddress(queue)));
            return endpoint;
        }

        public Task SubscribeQueueAsync<T>(string queue, Func<T, ConsumeContext<T>, Task> consumer,
            Action done = null, Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null) where T : class
        {
            return Subscribe(queue, new RabbitContextAdvancedConsumer<T>(consumer, done),
                typeof(RabbitContextAdvancedConsumer<T>), epCustom, busCustom);
        }

        public Task SubscribeConsumerAsync<T>(string queue,
            Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null)
        {
            return Subscribe(queue, null, typeof(T), epCustom, busCustom);
        }

        public void SubscribeConsumer<T>(string queue, Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null)
        {
            MassTransit.Util.TaskUtil.Await(SubscribeConsumerAsync<T>(queue, epCustom, busCustom));
        }

        public Task SubscribeConsumerAsync<T>(string queue, IConsumer<T> consumer,
            Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null) where T : class
        {
            return Subscribe(queue, consumer, consumer.GetType(), epCustom, busCustom);
        }

        public async Task PublishAsync<T>(string queue, T content) where T : class
        {
            var endpoint = await GetEndpoint(queue);
            await endpoint.Send(content);
        }

        public IBusControl GetBusControl() => _connection;

        public async Task PublishAsync<T>(T content)
            where T : class
        {
            await _connection.Publish(content);
        }

        public async Task SendAsync<T>(T content)
            where T : class
        {
            await _connection.Send(content);
        }

        public async Task<TResponse> RequestAsync<TSend, TResponse>(string queue, TSend content,
            TimeSpan timeout, CancellationToken cancellationToken = default)
            where TSend : class where TResponse : class
        {
            // TODO: Set timeout
            var client = _connection.CreateRequestClient<TSend, TResponse>(new Uri(GetAddress(queue)), timeout);
            var response = await client.Request(content, cancellationToken);
            return response;
        }

        public void SetupSsl(X509Certificate certificate, Action<IRabbitMqSslConfigurator, IRabbitMqHostConfigurator> sslConfigurator)
        {
            _certificate = certificate;
            _sslConfigurator = sslConfigurator;
        }

        public Task ConfigureRawAsync(string queue,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom)
        {
            Subscribe(queue, null, null, null, busCustom);
            return Task.CompletedTask;
        }
    }
}