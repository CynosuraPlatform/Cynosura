using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace Cynosura.Messaging
{
    public class DefaultConsumerConfigurator<T> : IConsumerConfigurator where T : class, IConsumer
    {
        private readonly string _queue;

        public DefaultConsumerConfigurator(string queue)
        {
            _queue = queue;
        }

        public void Configure(IRabbitMqBusFactoryConfigurator configurator, IServiceProvider serviceProvider)
        {
            configurator.ReceiveEndpoint(_queue, ep => ep.ConfigureConsumer<T>(serviceProvider));
        }
    }
}
