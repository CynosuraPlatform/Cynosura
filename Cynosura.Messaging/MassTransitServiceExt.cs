using System;
using System.Collections.Generic;
using Autofac;
using Cynosura.Core.Messagiing;
using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;

namespace Cynosura.Messaging
{
    public static class MassTransitServiceExt
    {
        public static void AddMessaging(this ContainerBuilder builder, 
            IConfiguration configuration, 
            Action<IContainerBuilderConfigurator> configure = null,
            Action<IRabbitMqBusFactoryConfigurator, IComponentContext> configureBus = null)
        {
            builder.RegisterType<MassTransitService>().As<IMessagingService>().SingleInstance();
            builder.AddMassTransit(x =>
            {
                configure?.Invoke(x);
                x.AddBus(context => MassTransitService.CreateBus(context, (configurator, ctxt) =>
                {
                    var configurators = ctxt.Resolve<IEnumerable<IConsumerConfigurator>>();
                    if (configurators != null)
                    {
                        foreach (var consumerConfigurator in configurators)
                        {
                            consumerConfigurator.Configure(configurator, ctxt);
                        }
                    }
                    configureBus(configurator, ctxt);
                }));
            });
        }
    }
}