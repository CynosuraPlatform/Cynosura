using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Cynosura.Core.Messaging;

namespace Cynosura.Messaging
{
    public class MassTransitService : IMessagingService
    {
        private readonly ILogger<MassTransitService> _logger;
        private readonly MassTransitServiceOptions _options;
        private readonly IBusRegistrationContext _context;
        private readonly IBusControl _bus;

        public MassTransitService(
            IBusRegistrationContext context,
            IBusControl bus,
            IOptions<MassTransitServiceOptions> options,
            ILogger<MassTransitService> logger)
        {
            _context = context;
            _bus = bus;
            _logger = logger;
            _options = options.Value;
        }

        public static IBusControl CreateBus(IBusRegistrationContext context, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureBus = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var options = context.GetRequiredService<IOptions<MassTransitServiceOptions>>().Value;
                sbc.Host(new Uri(options.ConnectionUrl), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                configureBus?.Invoke(context, sbc);
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
                var bus = CreateBus(_context);
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