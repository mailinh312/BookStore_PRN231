using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class OrderCreateDto
    {
        public string? UserId { get; set; }

        public string Receiver { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string? Note { get; set; }
        public decimal? TotalPrice { get; set; }

    }
}
