using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cynosura.Web.Menu
{
    public class MenuProvider : IMenuProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<MenuItem> _menuItems;

        public MenuProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _menuItems = LoadMenuItems();
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            return _menuItems;
        }

        private IEnumerable<MenuItem> LoadMenuItems()
        {
            var type = typeof(IMenuModule);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(t => (IMenuModule) (_serviceProvider.GetService(t) ?? Activator.CreateInstance(t) ?? throw new Exception($"Can't create {t}")));
            return modules.SelectMany(s => s.GetMenuItems());
        }
    }
}