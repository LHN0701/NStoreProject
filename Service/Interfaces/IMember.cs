using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IMember
    {
        Member AnonymousLogin(MemberModel.Input.AnonymousLogin input);

        Member Login(string UserName, string Password);

        MemberModel.Output.MemberInfo Register(MemberModel.Input.Register input);

        NotetiModel ConfirmEmail(string email);

        NotetiModel CheckEmail(string email);

        NotetiModel ChangePassword(MemberModel.Input.ChangePassword input);
    }
}