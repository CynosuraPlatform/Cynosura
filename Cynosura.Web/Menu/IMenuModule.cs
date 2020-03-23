using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Menu
{
    public interface IMenuModule
    {
        IEnumerable<MenuItem> GetMenuItems();
    }
}
