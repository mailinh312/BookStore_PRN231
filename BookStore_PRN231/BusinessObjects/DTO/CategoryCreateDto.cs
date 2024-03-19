using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class CategoryCreateDto
    {
        [Display(Name = "Tên thể loại")]
        [Required(ErrorMessage = "Tên thể loại không được để trống!")]
        public string CategoryName { get; set; }
    }
}
