using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ImportDetailCreateDto
    {
        [Required(ErrorMessage ="Chọn sản phẩm!")]
        public int? BookId { get; set; }

        [Required(ErrorMessage = "Nhập số lượng!")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Nhập giá nhập hàng!")]
        public decimal? InputPrice { get; set; }

        [Required(ErrorMessage = "Nhập giá bán!")]
        public decimal? OutputPrice { get; set; }
    }
}
