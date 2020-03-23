using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Menu
{
    public class MenuItem
    {
        public MenuItem(string route, string name, string cssClass = null, IEnumerable<string> roles = null)
        {
            Route = route;
            Name = name;
            CssClass = cssClass;
            Roles = roles;
        }

        public string Route { get; set; }
        public string Name { get; set; }
        public string CssClass { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
