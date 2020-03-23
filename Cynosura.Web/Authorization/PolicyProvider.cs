using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Web.Authorization
{
    public class PolicyProvider : IPolicyProvider
    {
        public void RegisterPolicies(AuthorizationOptions options)
        {
            var type = typeof(IPolicyModule);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(t => (IPolicyModule)Activator.CreateInstance(t));
            foreach (var module in modules)
            {
                module.RegisterPolicies(options);
            }
        }
    }
}
