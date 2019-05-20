using System;
using Autofac;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging
{
    public class EndpointConsumerConfigurator : IConsumerConfigurator
    {
        private readonly string _queue;
        private readonly Action<IReceiveEndpointConfigurator, IComponentContext> _configureEndpoint;

        public EndpointConsumerConfigurator(
            string queue,
            Action<IReceiveEndpointConfigurator, IComponentContext> configureEndpoint)
        {
            _queue = queue;
            _configureEndpoint = configureEndpoint;
        }

        public void Configure(IRabbitMqBusFactoryConfigurator configurator, IComponentContext context)
        {
            configurator.ReceiveEndpoint(_queue, ep => {
                _configureEndpoint(ep, context);
            });
        }
    }
}
