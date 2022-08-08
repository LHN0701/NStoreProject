using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class MemberModel
    {
        public class MemberBase
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Họ tên phải khác rỗng")]
            [Display(Name = "Họ tên")]
            public string Name { get; set; }

            [Display(Name = "Giới tính")]
            public bool Gender { get; set; }

            [Display(Name = "Ngày sinh")]
            [DataType(DataType.Date)]
            public DateTime Dob { get; set; }

            [Required(ErrorMessage = "Email phải khác rỗng")]
            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Điện thoại phải khác rỗng")]
            [Display(Name = "Điện thoại")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Mật khẩu phải khác rỗng")]
            [Display(Name = "Mật khẩu")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Địa chỉ phải khác rỗng")]
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }

            public bool Active { get; set; }
        }

        public class Input
        {
            public class Register : MemberBase
            {
            }

            public class ChangePassword
            {
                public string Email { get; set; }

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

            public class ConfirmEmail
            {
                [Required(ErrorMessage = "Email phải khác rỗng")]
                [Display(Name = "Email")]
                [DataType(DataType.EmailAddress)]
                [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
                public string Email { get; set; }
            }

            public class AnonymousLogin
            {
                public string Name { get; set; }

                public string Email { get; set; }
                public string AccountFrom { get; set; }
            }
        }

        public class Output
        {
            public class Register : MemberBase
            {
                [Required(ErrorMessage = "Xác nhận Mật khẩu phải khác rỗng")]
                [Display(Name = "Xác nhận mật khẩu")]
                [Compare("Password", ErrorMessage = "Xác nhận lại mật khẩu không đúng.")]
                [DataType(DataType.Password)]
                public string ConfirmPassword { get; set; }
            }

            public class MemberInfo : MemberBase
            {
                public string AccessToken { get; set; }
                public string Noteti { get; set; }
            }
        }
    }
}