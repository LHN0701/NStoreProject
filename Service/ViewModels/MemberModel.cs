using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class MemberModel
    {
        public class MemberBase
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool Gender { get; set; }

            public DateTime Dob { get; set; }

            public string Email { get; set; }
            public string Phone { get; set; }

            public string Password { get; set; }

            public string Address { get; set; }

            public bool Active { get; set; }
        }

        public class Input
        {
            public class Register : MemberBase
            { }

            public class ThongTinThanhVien
            {
                public int Id { get; set; }
            }

            public class ThongTinThayDoiMatKhau
            {
                public int Id { get; set; }
                public string Username { get; set; }
                public string MatKhauCu { get; set; }
                public string MatKhauMoi { get; set; }
            }

            public class LoginInfo
            {
                public string UserName { get; set; }

                public string Password { get; set; }

                public bool Remember { get; set; }
            }

            public class ConfirmEmail
            {
                public string Email { get; set; }
            }
        }

        public class Output
        {
            public class ThongTinThanhVienMoi : MemberBase
            {
                public string XacNhanMatKhau { get; set; }
            }

            public class MemberInfo : MemberBase
            {
                public string AccessToken { get; set; }
                public string Noteti { get; set; }
            }
        }
    }
}