using System;
using System.Collections.Generic;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Cynosura.Core.Messaging;

namespace Cynosura.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCynosuraMessaging(this IServiceCollection services,
            IConfiguration? configuration = null,
            Action<IServiceCollectionBusConfigurator>? configure = null)
        {
            services.AddSingleton<IMessagingService, MassTransitService>();
            if (configuration != null)
            {
                services.Configure<MassTransitServiceOptions>(configuration.GetSection("MassTransit"));
            }
            services.AddMassTransit(x =>
            {
                configure?.Invoke(x);
            });
            return services;
        }

        public static void AddRabbitMqBus(this IServiceCollectionBusConfigurator configure, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureBus = null)
        {
            configure.UsingRabbitMq((context, configurator) =>
            {
                var options = context.GetRequiredService<IOptions<MassTransitServiceOptions>>().Value;
                if (options.ConnectionUrl == null)
                {
                    throw new Exception("Specify ConnectionUrl");
                }
                configurator.Host(new Uri(options.ConnectionUrl), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                configureBus?.Invoke(context, configurator);
            });
        }

        public static void AddInMemoryBus(this IServiceCollectionBusConfigurator configure, Action<IBusRegistrationContext, IInMemoryBusFactoryConfigurator>? configureBus = null)
        {
            configure.UsingInMemory((context, configurator) =>
            {
                configureBus?.Invoke(context, configurator);
            });
        }
    }
}