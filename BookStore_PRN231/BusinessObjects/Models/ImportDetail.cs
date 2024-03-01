using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class ImportDetail
    {
        

        [Display(Name = "Mã phiếu nhập")]
        public int? ImportId { get; set; }

        [Required(ErrorMessage = "Chọn sản phẩm cần nhập thêm.")]
        [Display(Name = "Mã sản phẩm")]
        public int? BookId { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Nhập số lượng.")]

        public int? Quantity { get; set; }

        [Display(Name = "Giá nhập")]
        [Required(ErrorMessage = "Nhập giá nhập.")]
        public decimal? InputPrice { get; set; }

        [Display(Name = "Giá bán")]
        [Required(ErrorMessage = "Nhập giá bán.")]
        public decimal? OutputPrice { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal? TotalPrice { get; set; }

        public virtual Book? Book { get; set; }

        public virtual Import? Import { get; set; }
    }
}
