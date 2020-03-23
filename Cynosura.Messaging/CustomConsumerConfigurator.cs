using System;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging
{
    public class CustomConsumerConfigurator : IConsumerConfigurator
    {
        private readonly Action<IRabbitMqBusFactoryConfigurator, IServiceProvider> _configureBus;

        public CustomConsumerConfigurator(Action<IRabbitMqBusFactoryConfigurator, IServiceProvider> configureBus)
        {
            _configureBus = configureBus;
        }

        public void Configure(IRabbitMqBusFactoryConfigurator configurator, IServiceProvider serviceProvider)
        {
            _configureBus(configurator, serviceProvider);
        }
    }
}
