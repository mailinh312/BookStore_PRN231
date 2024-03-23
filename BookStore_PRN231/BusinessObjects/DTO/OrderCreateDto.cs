using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class OrderCreateDto
    {
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập người nhận.")]
        public string Receiver { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string Phone { get; set; }

        public string? Note { get; set; }
        public decimal? TotalPrice { get; set; } 

    }
}
