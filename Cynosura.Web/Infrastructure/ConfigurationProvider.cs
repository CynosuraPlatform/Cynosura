using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cynosura.Web.Infrastructure
{
    public class ConfigurationProvider<T> : IConfigurationProvider<T>
    {
        public void Configure(T configuration)
        {
            var type = typeof(IConfigurationModule<T>);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(t => (IConfigurationModule<T>?)Activator.CreateInstance(t));
            foreach (var module in modules)
            {
                module?.Configure(configuration);
            }
        }
    }
}
