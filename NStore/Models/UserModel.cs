using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class UserModel
    {
        public class UserBase
        {
            public int Id { get; set; }

            [Display(Name = "Họ tên nhân viên")]
            [Required(ErrorMessage = "Họ tên phải khác rỗng")]
            public string Name { get; set; }

            [Display(Name = "Giới tính")]
            public bool Gender { get; set; }

            [Display(Name = "Ngày sinh")]
            public DateTime Dob { get; set; }

            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Số CMND phải khác rỗng")]
            [Display(Name = "Số CMND")]
            public string Identification { get; set; }

            [Required(ErrorMessage = "Mật khẩu phải khác rỗng")]
            [Display(Name = "Mật khẩu")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Quyền hạn")]
            public string Role { get; set; }
        }

        public class Input
        {
            public class LoginInfo
            {
                [Display(Name = "UserName")]
                [Required(ErrorMessage = "Please enter your username.")]
                public string UserName { get; set; }

                [DataType(DataType.Password)]
                [Display(Name = "Password")]
                [Required(ErrorMessage = "Please enter your password.")]
                public string Password { get; set; }

                [Display(Name = "Ghi nhớ đăng nhập")]
                public bool Remember { get; set; }
            }

            public class ChangePassword
            {
                [Required(ErrorMessage = "Mật khẩu cũ phải khác rỗng.")]
                [Display(Name = "Mật khẩu cũ")]
                [DataType(DataType.Password)]
                public string OldPassword { get; set; }

                [Required(ErrorMessage = "Mật khẩu mới phải khác rỗng.")]
                [Display(Name = "Mật khẩu mới")]
                [DataType(DataType.Password)]
                public string NewPassword { get; set; }

                [Required(ErrorMessage = "Nhập lại Mật khẩu mới phải khác rỗng.")]
                [Display(Name = "Nhập lại Mật khẩu mới")]
                [DataType(DataType.Password)]
                [Compare("NewPassword", ErrorMessage = "Nhập lại Mật khẩu mới không chính xác.")]
                public string EnterNewPassword { get; set; }
            }

            public class ChangePasswordInfo
            {
                public int Id { get; set; }
                public string Username { get; set; }
                public string OldPassword { get; set; }
                public string NewPassword { get; set; }
            }

            public class DanhSachNhanVien
            {
                public bool QuanTri { get; set; }
            }
        }

        public class Output
        {
            public class UserInfo : UserBase
            {
                public string AccessToken { get; set; }
                public string Noteti { get; set; }
            }
        }
    }
}