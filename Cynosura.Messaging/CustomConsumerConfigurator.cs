using System;
using Autofac;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging
{
    public class CustomConsumerConfigurator : IConsumerConfigurator
    {
        private readonly Action<IRabbitMqBusFactoryConfigurator, IComponentContext> _configureBus;

        public CustomConsumerConfigurator(Action<IRabbitMqBusFactoryConfigurator, IComponentContext> configureBus)
        {
            _configureBus = configureBus;
        }

        public void Configure(IRabbitMqBusFactoryConfigurator configurator, IComponentContext context)
        {
            _configureBus(configurator, context);
        }
    }
}
