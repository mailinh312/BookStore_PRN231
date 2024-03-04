using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ImportDto
    {

        public int ImportId { get; set; }

        public decimal? TotalPrice { get; set; }

        public String? UserId { get; set; }

        public string UserName { get; set; }

        public DateTime ImportDate { get; set; }
    }
}
