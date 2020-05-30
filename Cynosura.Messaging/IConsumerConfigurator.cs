using System;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging
{
    public interface IConsumerConfigurator
    {
        void Configure(IRabbitMqBusFactoryConfigurator configurator, IServiceProvider serviceProvider);
    }
}
