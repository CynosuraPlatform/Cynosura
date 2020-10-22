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
            Action<IServiceCollectionBusConfigurator>? configure = null,
            Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureBus = null)
        {
            services.AddSingleton<IMessagingService, MassTransitService>();
            if (configuration != null)
            {
                services.Configure<MassTransitServiceOptions>(configuration.GetSection("MassTransit"));
            }
            services.AddMassTransit(x =>
            {
                configure?.Invoke(x);
                x.UsingRabbitMq((context, cfg) => { cfg.ConfigureEndpoints(context); });
                x.AddBus(context => MassTransitService.CreateBus(context, (context, configurator) =>
                {
                    configureBus?.Invoke(context, configurator);
                }));
            });
            return services;
        }
    }
}