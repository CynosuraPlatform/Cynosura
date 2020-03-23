using MassTransit.RabbitMqTransport;
using System;

namespace Cynosura.Messaging
{
    public interface IConsumerConfigurator
    {
        void Configure(IRabbitMqBusFactoryConfigurator configurator, IServiceProvider serviceProvider);
    }
}
