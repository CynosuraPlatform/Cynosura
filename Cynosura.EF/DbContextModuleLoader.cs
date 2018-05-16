using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.EF
{
    public static class DbContextModuleLoader
    {
        public static void CreateModelFromModules(this ModelBuilder builder)
        {
            var type = typeof(IDbContextModule);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(t => (IDbContextModule)Activator.CreateInstance(t));
            foreach (var dataContextModule in modules)
            {
                dataContextModule.CreateModel(builder);
            }
        }
    }
}
