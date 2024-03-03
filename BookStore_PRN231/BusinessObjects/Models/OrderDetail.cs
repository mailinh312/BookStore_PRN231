using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        [Display(Name = "Mã đơn hàng")]
        [ForeignKey(nameof(Order))]
        public int? OrderId { get; set; }

        [Required]
        [Display(Name = "Mã sản phẩm")]
        [ForeignKey(nameof(Book))]
        public int? BookId { get; set; }

        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }

        [Display(Name = "Giá tiền")]
        public decimal? Price { get; set; }

        public virtual Book? Book { get; set; }
        public virtual Order? Order { get; set; }
    }
}
