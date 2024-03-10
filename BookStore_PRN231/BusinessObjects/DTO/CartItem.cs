using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class CartItem
    {
        public int? BookId { get; set; }

        public string BookTitle {  get; set; }
        public decimal BookPrice { get; set; }

        public int? Quantity { get; set; }
    }
}
