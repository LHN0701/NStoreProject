using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUser
    {
        UserModel.Output.UserInfo Login(string UserName, string Password);

        List<User> GetAllUsers();

        User GetById(int id);

        NotetiModel ChangePassword(UserModel.Input.ChangePasswordInfo input);

        NotetiModel CreateUser(UserModel.UserBase input);

        NotetiModel UpdateUser(int id, UserModel.UserBase input);

        NotetiModel DeleteUser(int id);
    }
}