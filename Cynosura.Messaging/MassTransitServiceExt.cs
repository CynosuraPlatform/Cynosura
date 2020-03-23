using System;
using System.Collections.Generic;
using Cynosura.Core.Messaging;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Cynosura.Messaging
{
    public static class MassTransitServiceExt
    {
        public static void AddMessaging(this IServiceCollection services,
            Action<IServiceCollectionConfigurator> configure = null,
            Action<IRabbitMqBusFactoryConfigurator, IServiceProvider> configureBus = null)
        {
            services.AddSingleton<IMessagingService, MassTransitService>();
            services.AddMassTransit(x =>
            {
                configure?.Invoke(x);
                x.AddBus(context => MassTransitService.CreateBus(context, (configurator, ctxt) =>
                {
                    var configurators = ctxt.GetRequiredService<IEnumerable<IConsumerConfigurator>>();
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