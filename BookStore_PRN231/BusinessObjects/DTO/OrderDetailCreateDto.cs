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
    public class OrderDetailCreateDto
    {

        public int? BookId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

    }
}
