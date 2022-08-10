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
    public class UserService : IUser
    {
        private readonly NStoreContext _context;

        public UserService(NStoreContext context)
        {
            _context = context;
        }

        public NotetiModel ChangePassword(UserModel.Input.ChangePasswordInfo input)
        {
            var result = new NotetiModel();
            try
            {
                var changeUser = _context.Users.FirstOrDefault(x => x.Id.Equals(input.Id) && x.Identification.Equals(input.Username));
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

        public NotetiModel CreateUser(UserModel.UserBase input)
        {
            var result = new NotetiModel();

            var user = _context.Users.FirstOrDefault(x => x.Identification.Equals(input.Identification));
            if (user != null)
            {
                return new NotetiModel()
                {
                    IsSuccess = false,
                    Noteti = "Identification already exists!"
                };
            }
            if (DateTime.Compare(input.Dob, new DateTime(1900, 1, 1)) < 0)
            {
                return new NotetiModel()
                {
                    IsSuccess = false,
                    Noteti = "Date of birth must be greater than 01/01/1900!"
                };
            }
            var createUser = new User()
            {
                Name = input.Name,
                Gender = input.Gender,
                Dob = input.Dob,
                Address = input.Address,
                Identification = input.Identification,
                Password = input.Password,
                Role = input.Role
            };

            if (createUser != null)
            {
                _context.Add(createUser);
                var change = _context.SaveChanges();
                if (change > 0)
                {
                    result.IsSuccess = true;
                    result.Noteti = "Create user success!";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Create user fail!";
            }

            return result;
        }

        public NotetiModel DeleteUser(int id)
        {
            NotetiModel result = new();
            var user = _context.Users.FirstOrDefault(x => x.Id.Equals(id));

            _context.Users.Remove(user);
            var change = _context.SaveChanges();

            if (change > 0)
            {
                result.IsSuccess = true;
                result.Noteti = "Delete user success!";
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Delete user fail!";
            }

            return result;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id.Equals(id));
        }

        public UserModel.Output.UserInfo Login(string UserName, string Password)
        {
            var userInfo = new UserModel.Output.UserInfo();
            var user = _context.Users.FirstOrDefault(x => x.Identification.Equals(UserName));

            if (user == null)
            {
                userInfo.Noteti = "Account does not exist!";
                return userInfo;
            }
            if (user.Password != Password)
            {
                userInfo.Noteti = "Password incorrect!";
                return userInfo;
            }

            Utilities.PropertyCopier<User, UserModel.Output.UserInfo>.Copy(user, userInfo);

            return userInfo;
        }

        public NotetiModel UpdateUser(int id, UserModel.UserBase input)
        {
            var result = new NotetiModel();

            var user = _context.Users.FirstOrDefault(x => x.Id.Equals(id));
            if (user == null)
            {
                result.IsSuccess = false;
                result.Noteti = "User have not already exists!";
            }
            else if (DateTime.Compare(input.Dob, new DateTime(1900, 1, 1)) < 0)
            {
                result.IsSuccess = false;
                result.Noteti = "Date of birth must be greater than 01/01/1900!";
            }
            else
            {
                user.Name = input.Name;
                user.Gender = input.Gender;
                user.Dob = input.Dob;
                user.Address = input.Address;
                user.Identification = input.Identification;
                user.Password = input.Password;
                user.Role = input.Role;

                var change = _context.SaveChanges();
                if (change > 0)
                {
                    result.IsSuccess = true;
                    result.Noteti = "Update user success!";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Noteti = "Update user fail!";
                }
            }

            return result;
        }
    }
}