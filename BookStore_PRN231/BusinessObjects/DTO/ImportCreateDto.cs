using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ImportCreateDto
    {

        public decimal? TotalPrice { get; set; }

        public String? UserId { get; set; }

    }
}
