using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class CartItem
    {
        public int? OrderId { get; set; }

        public int? BookId { get; set; }

        public string? BookTitle { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
