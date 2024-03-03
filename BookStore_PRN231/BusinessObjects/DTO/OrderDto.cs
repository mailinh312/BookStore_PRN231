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
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string? UserId { get; set; }

        public string? Username { get; set; }
        public DateTime OrderDate { get; set; }
        public string Receiver { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string? Note { get; set; }
        public decimal? TotalPrice { get; set; }

        public int? StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
