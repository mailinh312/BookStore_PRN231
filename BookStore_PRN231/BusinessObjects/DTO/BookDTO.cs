using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }

        public string? Title { get; set; }

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public decimal? OriginPrice { get; set; }

        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }

        public string? AuthorName { get; set; }

        public string? Publisher { get; set; }

        public DateTime? PublishDate { get; set; }
        public int? StockQuantity { get; set; }

        [Display(Name = "Link ảnh")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
    }
}

