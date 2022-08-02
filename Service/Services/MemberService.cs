using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class MemberService : IMember
    {
        private readonly NStoreContext _context;

        public MemberService(NStoreContext context)
        {
            _context = context;
        }

        public Member Login(string UserName, string Password)
        {
            var member = new Member();
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    member = _context.Members.FirstOrDefault(x => x.Email.Equals(UserName)
                                        && x.Password.Equals(Password) && x.Active.Equals(true));
                }
                catch { }
            }
            return member;
        }

        public MemberModel.Output.MemberInfo Register(MemberModel.Input.Register input)
        {
            var member = _context.Members.FirstOrDefault(x => x.Email.Equals(input.Email));
            if (member != null)
            {
                return new MemberModel.Output.MemberInfo()
                {
                    Noteti = "Tài khoản đã tồn tại"
                };
            }

            var user = new Member()
            {
                Name = input.Name,
                Dob = input.Dob,
                Email = input.Email,
                Phone = input.Phone,
                Gender = input.Gender,
                Password = input.Password,
                Address = input.Address
            };

            if (user != null)
            {
                _context.Members.Add(user);
                _context.SaveChanges();

                return new MemberModel.Output.MemberInfo()
                {
                    Name = user.Name,
                    Password = user.Password
                };
            }

            return new MemberModel.Output.MemberInfo()
            {
                Noteti = "Tạo tài khoản thất bại"
            };
        }

        public NotetiModel ConfirmEmail(string email)
        {
            var noteti = new NotetiModel();
            var member = _context.Members.FirstOrDefault(x => x.Email.Equals(email));
            if (member != null)
            {
                member.Active = true;
                _context.SaveChanges();

                noteti.IsSuccess = true;
                noteti.Noteti = "Active email success";
            }
            return noteti;
        }

        public NotetiModel CheckEmail(string email)
        {
            var noteti = new NotetiModel();
            var member = _context.Members.FirstOrDefault(x => x.Email.Equals(email));
            if (member == null)
            {
                noteti.Noteti = "Can not find email";
                noteti.IsSuccess = false;

                return noteti;
            }
            noteti.IsSuccess = true;

            return noteti;
        }
    }
}