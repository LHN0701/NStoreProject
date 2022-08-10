using Service.Common;
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

        public MemberModel.Output.MemberInfo Login(string UserName, string Password)
        {
            var memberInfo = new MemberModel.Output.MemberInfo();
            var member = _context.Members.FirstOrDefault(x => x.Email.Equals(UserName)
                                 && x.Active.Equals(true));

            if (member == null)
            {
                memberInfo.Noteti = "Account does not exist!";
                return memberInfo;
            }
            if (member.Password != Password)
            {
                memberInfo.Noteti = "Password incorrect!";
                return memberInfo;
            }

            Utilities.PropertyCopier<Member, MemberModel.Output.MemberInfo>.Copy(member, memberInfo);

            return memberInfo;
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

            if (DateTime.Compare(input.Dob, new DateTime(1900, 1, 1)) < 0)
            {
                return new MemberModel.Output.MemberInfo()
                {
                    Noteti = "Date of birth must be greater than 01/01/1900"
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
                Address = input.Address,
                AccountFrom = "Store"
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

        public Member AnonymousLogin(MemberModel.Input.AnonymousLogin input)
        {
            var user = new Member()
            {
                Name = input.Name,
                Dob = null,
                Email = input.Email,
                Phone = null,
                Gender = null,
                Password = Utilities.RandomPassword(),
                Address = null,
                Active = true,
                AccountFrom = input.AccountFrom
            };

            if (user != null)
            {
                _context.Members.Add(user);
                _context.SaveChanges();
            }

            return user;
        }

        public NotetiModel ChangePassword(MemberModel.Input.ChangePassword input)
        {
            var result = new NotetiModel();
            try
            {
                var changeUser = _context.Members.FirstOrDefault(x => x.Email.Equals(input.Email));
                if (changeUser != null)
                {
                    if (changeUser.Password == input.OldPassword)
                    {
                        changeUser.Password = input.NewPassword;
                        var change = _context.SaveChanges();
                        if (change > 0)
                        {
                            result.IsSuccess = true;
                            result.Noteti = "Change password Success";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Noteti = "Change password incorrect";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Noteti = "Old Password incorrect";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Noteti = "Error: " + ex.Message;
            }

            return result;
        }
    }
}