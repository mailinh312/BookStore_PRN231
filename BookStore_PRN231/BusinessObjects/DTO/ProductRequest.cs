using BusinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ProductRequest: PagingRequestBase
    {
        public int CategoryId { get; set; }

        public int AuthorId { get; set; }
        public string? SortType { get; set; }
    }
}
