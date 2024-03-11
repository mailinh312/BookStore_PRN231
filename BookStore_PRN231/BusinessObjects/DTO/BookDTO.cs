using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class BookDto
    {
        [Display(Name = "Mã sản phẩm")]
        public int BookId { get; set; }

        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề sách không được để trống!")]
        public string? Title { get; set; }

        [Display(Name = "Phân loại")]
        [Required(ErrorMessage = "Chọn phân loại!")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn phân loại")]
        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        [Display(Name = "Giá nhập")]
        [Required(ErrorMessage = "Giá nhập sản phẩm không được để trống!")]
        public decimal? OriginPrice { get; set; }

        [Display(Name = "Giá bán")]
        [Required(ErrorMessage = "Giá sản phẩm không được để trống!")]
        public decimal? Price { get; set; }

        [Display(Name = "Tác giả")]
        [Required(ErrorMessage = "Chọn tác giả!")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn tác giả")]
        public int? AuthorId { get; set; }

        public string? AuthorName { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Thêm mô tả!")]
        public string? Description { get; set; }

        [Display(Name = "Nhà xuất bản")]
        [Required(ErrorMessage = "Nhà xuất bản không được để trống!")]
        public string? Publisher { get; set; }

        [Required(ErrorMessage = "Ngày xuất bản không được để trống!")]
        [Display(Name = "Ngày xuất bản")]
        public DateTime? PublishDate { get; set; }
        [Display(Name = "Số lượng trong kho")]
        [Required(ErrorMessage = "Số lượng trong kho không được để trống!")]
        public int? StockQuantity { get; set; }

        [Display(Name = "Link ảnh")]
        
        public string? ImageUrl { get; set; }

        public bool Active { get; set; }
    }
}

