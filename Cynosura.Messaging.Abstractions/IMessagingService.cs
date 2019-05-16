using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging.Abstractions
{
    public interface IMessagingService
    {
        void Connect(Action<IRabbitMqHostConfigurator> advanced = null);
        void Start();
        void Stop(TimeSpan stopTimeout);

        Task SubscribeQueueAsync<T>(string queue, Func<T, ConsumeContext<T>, Task> consumer,
            Action done = null, Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null) where T : class;

        Task SubscribeConsumerAsync<T>(string queue,
            Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null);

        void SubscribeConsumer<T>(string queue,
            Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null);

        Task SubscribeConsumerAsync<T>(string queue,
            IConsumer<T> consumer,
            Action<IRabbitMqReceiveEndpointConfigurator> epCustom = null,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom = null) where T : class;

        Task PublishAsync<T>(string queue, T content) where T : class;
        IBusControl GetBusControl();

        Task PublishAsync<T>(T content)
            where T : class;

        Task SendAsync<T>(T content)
            where T : class;

        Task<TResponse> RequestAsync<TSend, TResponse>(string queue, TSend content,
            TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
            where TSend : class where TResponse : class;

        void SetupSsl(X509Certificate certificate, Action<IRabbitMqSslConfigurator, IRabbitMqHostConfigurator> sslConfigurator);
        Task ConfigureRawAsync(string queue, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom);
    }
}
