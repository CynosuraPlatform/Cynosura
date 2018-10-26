using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Infrastructure.Menu
{
    public interface IMenuProvider
    {
        IEnumerable<MenuItem> GetMenuItems();
    }
}
