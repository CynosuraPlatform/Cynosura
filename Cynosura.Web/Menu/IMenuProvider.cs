using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Menu
{
    public interface IMenuProvider
    {
        IEnumerable<MenuItem> GetMenuItems();
    }
}
