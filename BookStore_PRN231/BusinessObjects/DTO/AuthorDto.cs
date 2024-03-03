using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }
       
        public string? AuthorName { get; set; }

        public string? Bio { get; set; }

        public bool Active { get; set; }
    }
}
