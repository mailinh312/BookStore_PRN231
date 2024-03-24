using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class Top3Category
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? SoldQuantity { get; set; }
    }
}
