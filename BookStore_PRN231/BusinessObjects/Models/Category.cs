using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [Display(Name = "Mã thể loại")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên thể loại không được để trống!")]
        [Display(Name = "Tên thể loại")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
