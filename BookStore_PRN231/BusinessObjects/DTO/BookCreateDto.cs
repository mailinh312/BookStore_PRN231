using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class BookCreateDto
    {

        public string? Title { get; set; }

        public int? CategoryId { get; set; }

        public decimal? OriginPrice { get; set; }

        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }

        public string? Description { get; set; }

        public string? Publisher { get; set; }

        public DateTime? PublishDate { get; set; }
        public int? StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

    }
}
