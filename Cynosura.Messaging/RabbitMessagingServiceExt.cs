using Cynosura.Messaging.Abstractions;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Messaging
{
    public static class RabbitMessagingServiceExt
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit();
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IMessagingService, MessagingService>();

            services.Configure<RabbitMessagingServiceOptions>(configuration.GetSection("Rabbit"));
            return services;
        }
    }
}