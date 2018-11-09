using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Core.Services.Models
{
    public class PageModel<T>
    {
        public IEnumerable<T> PageItems { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPageIndex { get; set; }

        public PageModel()
        {

        }

        public PageModel(IEnumerable<T> pageItems, int totalItems, int currentPageIndex)
        {
            PageItems = pageItems;
            TotalItems = totalItems;
            CurrentPageIndex = currentPageIndex;
        }
    }
}
