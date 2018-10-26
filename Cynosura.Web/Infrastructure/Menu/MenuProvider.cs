using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cynosura.Web.Infrastructure.Menu
{
    public class MenuProvider : IMenuProvider
    {
        private readonly IEnumerable<MenuItem> _menuItems;

        public MenuProvider()
        {
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
                .Select(t => (IMenuModule)Activator.CreateInstance(t));
            return modules.SelectMany(s=>s.GetMenuItems());
        }
    }
}
