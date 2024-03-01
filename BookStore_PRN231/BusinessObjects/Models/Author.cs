using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [Display(Name = "Mã tác giả")]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Tên tác giả không được để trống!")]

        [Display(Name = "Tên tác giả")]
        public string? AuthorName { get; set; }

        [Display(Name = "Mô tả")]
        public string? Bio { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
