using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Web.Infrastructure.Authorization
{
    public class PolicyProvider : IPolicyProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public PolicyProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void RegisterPolicies(AuthorizationOptions options)
        {
            var type = typeof(IPolicyModule);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(t => (IPolicyModule)(_serviceProvider.GetService(t) ?? Activator.CreateInstance(t)));
            foreach (var module in modules)
            {
                module.RegisterPolicies(options);
            }
        }
    }
}
