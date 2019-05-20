using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Cynosura.Core.Messagiing;
using GreenPipes.Util;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cynosura.Messaging
{
    public class MassTransitService : IMessagingService
    {
        private readonly ILogger<MassTransitService> _logger;
        private readonly MassTransitServiceOptions _options;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IBusControl _bus;

        public MassTransitService(
            ILifetimeScope lifetimeScope,
            IBusControl bus,
            IOptions<MassTransitServiceOptions> options,
            ILogger<MassTransitService> logger)
        {
            _lifetimeScope = lifetimeScope;
            _bus = bus;
            _logger = logger;
            _options = options.Value;
        }

        public static IBusControl CreateBus(IComponentContext context, Action<IRabbitMqBusFactoryConfigurator, IComponentContext> configureBus = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var options = context.Resolve<IOptions<MassTransitServiceOptions>>().Value;
                var host = sbc.Host(new Uri(options.ConnectionUrl), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                configureBus?.Invoke(sbc, context);

                sbc.UseExtensionsLogging(context.Resolve<ILoggerFactory>());
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await WaitForTransportAsync();
            await _bus.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.StopAsync(cancellationToken);
        }

        private async Task WaitForTransportAsync()
        {
            await RetryHelper.TryAsync(async () =>
            {
                var bus = CreateBus(_lifetimeScope);
                await bus.StartAsync();
                await bus.StopAsync();
            }, 5, TimeSpan.FromSeconds(10), _logger);
        }

        private string GetAddress(string queue) =>
            $"{_options.ConnectionUrl}/{queue}";

        private async Task<ISendEndpoint> GetEndpoint(string queue)
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri(GetAddress(queue)));
            return endpoint;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            await _bus.Publish(message);
        }

        public async Task PublishAsync<T>(T message, TimeSpan timeToLive) where T : class
        {
            await _bus.Publish(message, x => x.TimeToLive = timeToLive);
        }

        public async Task SendAsync<T>(string queue, T message) where T : class
        {
            var endpoint = await GetEndpoint(queue);
            await endpoint.Send(message);
        }
    }
}