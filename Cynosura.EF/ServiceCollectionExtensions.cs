using Cynosura.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCynosuraEF(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            return services;
        }
    }
}
