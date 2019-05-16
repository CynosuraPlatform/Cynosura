using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Cynosura.Messaging
{
    public class ConsumerRegistration
    {
        public ConsumerRegistration(string queue, IConsumer consumer, Type consumerType, Action<IRabbitMqReceiveEndpointConfigurator> custom, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> busCustom)
        {
            Queue = queue;
            Consumer = consumer;
            ConsumerType = consumerType;
            Custom = custom;
            BusCustom = busCustom;
        }

        public string Queue { get; set; }
        public IConsumer Consumer { get; set; }
        public Type ConsumerType { get; set; }
        public Action<IRabbitMqReceiveEndpointConfigurator> Custom { get; set; }
        public Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> BusCustom { get; set; }
    }
}