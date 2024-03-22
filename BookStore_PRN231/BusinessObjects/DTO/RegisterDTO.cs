using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Họ tên không được để trống!")]
        public string? FullName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống!")]
        public string? UserName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        public string? Password
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp!")]
        public string? RePassword
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Email không được để trống!")]
        public string? Email
        {
            get;
            set;
        }

    }
}
