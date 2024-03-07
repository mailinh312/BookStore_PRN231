using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class Cart
    {
        public List<OrderDetailCreateDto> Items { get; set; }
        public decimal? TotalPrice { get; set; }

        public Cart()
        {
            TotalPrice = 0;
            foreach (var item in Items)
            {
                TotalPrice += item.Price;
            }
        }
    }
}
