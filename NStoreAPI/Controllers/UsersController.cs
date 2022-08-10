using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Auth;
using Service.Common;
using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Service.Auth.JwtAuthManager;

namespace NStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _iUser;
        private readonly IJwtAuthManager _jwtAuthManager;

        public UsersController(IUser iUser, IJwtAuthManager jwtAuthManager)
        {
            _iUser = iUser;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost("Login")]
        public IActionResult Login(UserModel.Input.LoginInfo input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _iUser.Login(input.UserName, input.Password);
            if (user != null)
            {
                var userInfo = new UserInfo
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Identification,
                    UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
                };
                if (user.Identification != null)
                {
                    user.AccessToken = _jwtAuthManager.CreateToken(userInfo);
                }
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPost("Logout")]
        public bool Logout(string input)
        {
            try
            {
                _jwtAuthManager.RemoveTokenByUserName(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost("GetAllUser")]
        public List<User> GetAllUser()
        {
            var listUser = _iUser.GetAllUsers().ToList();

            return listUser;
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(UserModel.Input.ChangePasswordInfo input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iUser.ChangePassword(input);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(UserModel.UserBase input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iUser.CreateUser(input);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, UserModel.UserBase input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iUser.UpdateUser(id, input);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iUser.DeleteUser(id);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iUser.GetById(id);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}