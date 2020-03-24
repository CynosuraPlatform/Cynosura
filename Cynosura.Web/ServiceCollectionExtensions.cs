using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Web.Infrastructure;
using Cynosura.Web.Authorization;
using Cynosura.Web.Menu;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCynosuraWeb(this IServiceCollection services)
        {
            services.AddTransient<ApiExceptionFilterAttribute>();
            services.AddTransient<IExceptionHandler, ServiceExceptionHandler>();
            services.AddSingleton<IMenuProvider, MenuProvider>();
            services.AddSingleton<IPolicyProvider, PolicyProvider>();

            return services;
        }
    }
}
