using System;
using System.Collections.Generic;

namespace ExpenseManager.Web.Models
{
    public class PaginatedViewModel<T>
    {
        public List<T> Items { get; set; } = [];

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages
        {
            get
            {
                return PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
