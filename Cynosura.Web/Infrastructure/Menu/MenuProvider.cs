using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cynosura.Web.Infrastructure.Menu
{
    public class MenuProvider : IMenuProvider
    {
        private readonly IList<MenuItem> _menuItems;

        public MenuProvider()
        {
            _menuItems = LoadMenuItems();
        }

        public IList<MenuItem> GetMenuItems()
        {
            return _menuItems;
        }

        private IList<MenuItem> LoadMenuItems()
        {
            var menuItems = new List<MenuItem>();
            var type = typeof(IMenuModule);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(t => (IMenuModule)Activator.CreateInstance(t));
            foreach (var menuModule in modules)
            {
                menuItems.AddRange(menuModule.GetMenuItems());
            }
            return menuItems;
        }
    }
}
