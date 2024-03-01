using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Display(Name = "Mã đơn hàng")]
        public int OrderId { get; set; }
        public String? UserId { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Người nhận")]
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận!")]
        public string Receiver { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public string Phone { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        [Display(Name = "Tổng thanh toán")]
        public decimal? TotalPrice { get; set; }

        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        [Display(Name = "Người đặt hàng")]
        public virtual AppUser? User { get; set; }
        public virtual Status? Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
