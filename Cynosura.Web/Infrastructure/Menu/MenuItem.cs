using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Infrastructure.Menu
{
    public class MenuItem
    {
        public string Route { get; set; }
        public string Name { get; set; }
        public string CssClass { get; set; }
        public IList<string> Roles { get; set; }
    }
}
