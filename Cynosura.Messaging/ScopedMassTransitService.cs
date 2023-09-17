using Cynosura.Core.Messaging;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Messaging
{
    public class ScopedMassTransitService : IScopedMessagingService
    {
        private readonly IScopedClientFactory _scopedClientFactory;
        private readonly MassTransitServiceOptions _options;

        public ScopedMassTransitService(IScopedClientFactory scopedClientFactory,
            IOptions<MassTransitServiceOptions> options)
        {
            _scopedClientFactory = scopedClientFactory;
            _options = options.Value;
        }

        private string GetAddress(string queue) =>
            $"{_options.ConnectionUrl}/{queue}";

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(string queue, TRequest message)
            where TRequest : class
            where TResponse : class
        {
            var requestClient = _scopedClientFactory.CreateRequestClient<TRequest>(new Uri(GetAddress(queue)));
            var response = await requestClient.GetResponse<TResponse>(message);
            return response.Message;
        }
    }
}
