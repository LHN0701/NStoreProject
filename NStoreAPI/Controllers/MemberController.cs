using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Service.Auth;
using Service.Common;
using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMember _iMember;
        private readonly IJwtAuthManager _jwtAuthManager;

        public MemberController(IMember iMember, IJwtAuthManager jwtAuthManager)
        {
            _iMember = iMember;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost("AnonymousLogin")]
        public IActionResult AnonymousLogin(MemberModel.Input.AnonymousLogin input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = _iMember.AnonymousLogin(input);

            MemberModel.Output.MemberInfo memberinfo = new();
            if (member != null)
            {
                Utilities.PropertyCopier<Member, MemberModel.Output.MemberInfo>.Copy(member, memberinfo);
                var memberInfo = new UserInfo
                {
                    Id = member.Id,
                    Name = member.Name,
                    Username = member.Email,
                    UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
                };
                if (member.Email != null)
                {
                    memberinfo.AccessToken = _jwtAuthManager.CreateToken(memberInfo);
                }
            }
            return Ok(memberinfo);
        }

        [HttpPost("Login")]
        public IActionResult Login(MemberModel.Input.LoginInfo input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = _iMember.Login(input.UserName, input.Password);
            if (member != null)
            {
                var memberInfo = new UserInfo
                {
                    Id = member.Id,
                    Name = member.Name,
                    Username = member.Email,
                    UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
                };
                if (member.Email != null)
                {
                    member.AccessToken = _jwtAuthManager.CreateToken(memberInfo);
                }
            }
            return Ok(member);
        }

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

        [HttpPost("Register")]
        public IActionResult Register(MemberModel.Input.Register input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iMember.Register(input);

            if (result.Noteti == null)
            {
                return Ok(result);
            }

            return Ok(result);
        }

        [HttpPost("CheckEmail")]
        public IActionResult CheckEmail(MemberModel.Input.ConfirmEmail input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iMember.CheckEmail(input.Email);

            return Ok(result);
        }

        [HttpPost("ConfirmEmail")]
        public IActionResult ConfirmEmail(MemberModel.Input.ConfirmEmail input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iMember.ConfirmEmail(input.Email);

            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(MemberModel.Input.ChangePassword input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iMember.ChangePassword(input);

            return Ok(result);
        }
    }
}