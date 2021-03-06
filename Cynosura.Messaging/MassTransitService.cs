using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Options;
using Cynosura.Core.Messaging;

namespace Cynosura.Messaging
{
    public class MassTransitService : IMessagingService
    {
        private readonly MassTransitServiceOptions _options;
        private readonly IBusControl _bus;

        public MassTransitService(
            IBusControl bus,
            IOptions<MassTransitServiceOptions> options)
        {
            _bus = bus;
            _options = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.StopAsync(cancellationToken);
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