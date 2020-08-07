using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Core.Services.Models
{
    public class PageModel<T>
    {
        public List<T> PageItems { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPageIndex { get; set; }

        public PageModel()
        {
            PageItems = new List<T>();
        }

        public PageModel(List<T> pageItems, int totalItems, int currentPageIndex)
        {
            PageItems = pageItems;
            TotalItems = totalItems;
            CurrentPageIndex = currentPageIndex;
        }
    }
}
