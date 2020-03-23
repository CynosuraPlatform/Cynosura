using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging
{
    public class EndpointConsumerConfigurator : IConsumerConfigurator
    {
        private readonly string _queue;
        private readonly Action<IReceiveEndpointConfigurator, IServiceProvider> _configureEndpoint;

        public EndpointConsumerConfigurator(
            string queue,
            Action<IReceiveEndpointConfigurator, IServiceProvider> configureEndpoint)
        {
            _queue = queue;
            _configureEndpoint = configureEndpoint;
        }

        public void Configure(IRabbitMqBusFactoryConfigurator configurator, IServiceProvider serviceProvider)
        {
            configurator.ReceiveEndpoint(_queue, ep => {
                _configureEndpoint(ep, serviceProvider);
            });
        }
    }
}
