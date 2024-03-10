using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }
        public decimal? TotalPrice { get; set; }

        public Cart()
        {
            Items = new List<CartItem>();
            TotalPrice = 0;
        }
    }
}
