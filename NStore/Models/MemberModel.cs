using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NStore.Models
{
    public class MemberModel
    {
        public class MemberBase
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Please enter your name.")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Please choose your Gender.")]
            [Display(Name = "Gender")]
            public bool Gender { get; set; }

            [Required(ErrorMessage = "Please enter your date of birth.")]
            [Display(Name = "Date of birth")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime Dob { get; set; }

            [Required(ErrorMessage = "Please enter your email.")]
            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Invalid email.")]
            public string Email { get; set; }

            [Display(Name = "Phone")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Please enter your password.")]
            [Display(Name = "Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Address")]
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

                [Required(ErrorMessage = "Please enter your password.")]
                [Display(Name = "OldPassword")]
                [DataType(DataType.Password)]
                public string OldPassword { get; set; }

                [Required(ErrorMessage = "Please enter your new password.")]
                [Display(Name = "NewPassword")]
                [DataType(DataType.Password)]
                public string NewPassword { get; set; }

                [Required(ErrorMessage = "Please enter your new password.")]
                [Display(Name = "EnterNewPassword")]
                [DataType(DataType.Password)]
                [Compare("NewPassword", ErrorMessage = "Confirm password incorrect.")]
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

                [Display(Name = "Remember")]
                public bool Remember { get; set; }
            }

            public class ConfirmEmail
            {
                [Required(ErrorMessage = "Please enter your email.")]
                [Display(Name = "Email")]
                [DataType(DataType.EmailAddress)]
                [EmailAddress(ErrorMessage = "Email format incorrect")]
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
                [Required(ErrorMessage = "Please enter your password.")]
                [Display(Name = "ConfirmPassword")]
                [Compare("Password", ErrorMessage = "Confirm password incorrect.")]
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