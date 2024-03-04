using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ImportDetailCreateDto
    {
        
        public int? ImportId { get; set; }

        public int? BookId { get; set; }

        public int? Quantity { get; set; }

        public decimal? InputPrice { get; set; }

        public decimal? OutputPrice { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}
