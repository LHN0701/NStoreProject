using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class UserModel
    {
        public class UserBase
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Gender { get; set; }
            public DateTime Dob { get; set; }
            public string Address { get; set; }
            public string Identification { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class Input
        {
            public class LoginInfo
            {
                public string UserName { get; set; }
                public string Password { get; set; }
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