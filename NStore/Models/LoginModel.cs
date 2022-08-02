using System.ComponentModel.DataAnnotations;

namespace NStore.Models
{
    public class LoginModel
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
}