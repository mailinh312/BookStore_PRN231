using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Import
    {
        public Import()
        {
            ImportDetails = new HashSet<ImportDetail>();
        }

        [Key]
        [Display(Name = "Mã phiếu nhập")]
        [Required]
        public int ImportId { get; set; }

        [Display(Name = "Tổng thanh toán")]

        [Required(ErrorMessage = "Vui lòng nhập tổng thanh toán!")]
        public decimal? TotalPrice { get; set; }

        public String? UserId { get; set; }

        [Required]
        public DateTime ImportDate { get; set; }

        public virtual AppUser? User { get; set; }
        public virtual ICollection<ImportDetail> ImportDetails { get; set; }
    }
}
