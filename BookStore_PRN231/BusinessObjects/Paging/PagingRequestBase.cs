using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Paging
{
    public class PagingRequestBase
    {
        public string? KeyWords { get; set; }
        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 8;
        public int Skip => (Page - 1) * ItemsPerPage;
        public int Take { get => ItemsPerPage; }
        public string? SortField { get; set; }
    }
}
