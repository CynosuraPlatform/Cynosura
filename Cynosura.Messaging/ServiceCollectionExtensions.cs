using System;
using System.Collections.Generic;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Cynosura.Core.Messaging;

namespace Cynosura.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCynosuraMessaging(this IServiceCollection services,
            IConfiguration? configuration = null,
            Action<IServiceCollectionConfigurator>? configure = null,
            Action<IRabbitMqBusFactoryConfigurator, IServiceProvider>? configureBus = null)
        {
            services.AddSingleton<IMessagingService, MassTransitService>();
            if (configuration != null)
            {
                services.Configure<MassTransitServiceOptions>(configuration.GetSection("MassTransit"));
            }
            services.AddMassTransit(x =>
            {
                configure?.Invoke(x);
                x.AddBus(context => MassTransitService.CreateBus(context, (configurator, sp) =>
                {
                    configurator.SetLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
                    var configurators = sp.GetServices<IConsumerConfigurator>();
                    if (configurators != null)
                    {
                        foreach (var consumerConfigurator in configurators)
                        {
                            consumerConfigurator.Configure(configurator, sp);
                        }
                    }
                    configureBus?.Invoke(configurator, sp);
                }));
            });
            return services;
        }
    }
}