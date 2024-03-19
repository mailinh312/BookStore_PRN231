using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class AuthorCreateDto
    {
        [Display(Name ="Tên tác giả")]
        [Required(ErrorMessage ="Tên tác giả không được để trống!")]
        public string? AuthorName { get; set; }

        [Display(Name ="Tiểu sử")]
        public string? Bio { get; set; }
    }
}
