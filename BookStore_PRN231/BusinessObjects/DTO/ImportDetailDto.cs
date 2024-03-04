using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ImportDetailDto
    {
        public int ImportDetailId { get; set; }

        public int? ImportId { get; set; }

        public int? BookId { get; set; }

        public string? BookTitle { get; set; }
        public int? Quantity { get; set; }

        public decimal? InputPrice { get; set; }

        public decimal? OutputPrice { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}
