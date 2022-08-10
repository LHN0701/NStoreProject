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

            [Display(Name = "Name")]
            [Required(ErrorMessage = "Name must be non-empty.")]
            public string Name { get; set; }

            [Display(Name = "Gender")]
            [Required(ErrorMessage = "Gender must be non-empty.")]
            public bool Gender { get; set; }

            [Display(Name = "Date of birth")]
            [Required(ErrorMessage = "Date of birth must be non-empty.")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime Dob { get; set; }

            [Display(Name = "Address")]
            [Required(ErrorMessage = "Address must be non-empty.")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Identification must be non-empty.")]
            [Display(Name = "Identification")]
            public string Identification { get; set; }

            [Required(ErrorMessage = "Password must be non-empty.")]
            [Display(Name = "Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Role")]
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

                [Display(Name = "Remember")]
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