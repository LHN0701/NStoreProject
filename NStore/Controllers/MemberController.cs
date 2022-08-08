using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NStore.Controllers
{
    public class MemberController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new MemberModel.Input.LoginInfo();

            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(string username, string password, bool remember = false, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            if (username == null) username = "";
            if (password == null) password = "";

            var input = new MemberModel.Input.LoginInfo { UserName = username, Password = password, Remember = remember };

            var member = Utilities.SendDataRequest<MemberModel.Output.MemberInfo>(ConstantValues.Member.Login, input);
            HttpContext.Session.Remove("Member");
            if (member != null)
            {
                if (member.Id > 0)
                {
                    bool logined = LoginUser(member, remember);
                    if (logined)
                        HttpContext.Session.Set<MemberModel.Output.MemberInfo>("Member", member);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    TempData["result"] = member.Noteti;
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Logout()
        {
            //var user = HttpContext.Session.Get<MemberModel.Output.MemberInfo>("Member");
            //var input = new UserModel.Input.LoginInfo { UserName = user.Email, Password = user.Password };
            var input = User.Claims.FirstOrDefault(x => x.Type == "NAME").Value;
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Member");
            //HttpContext.Session.Clear();
            Utilities.SendDataRequest<bool>(ConstantValues.Member.Logout, input);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AnonymousLogin(MemberModel.Input.AnonymousLogin request)
        {
            var input = new MemberModel.Input.AnonymousLogin()
            {
                Email = request.Email,
                Name = request.Name,
                AccountFrom = request.AccountFrom
            };
            var anonumousLogin = Utilities.SendDataRequest<MemberModel.Output.MemberInfo>(ConstantValues.Member.AnonymousLogin, input);

            bool logined = LoginUser(anonumousLogin, false);
            if (logined)
                HttpContext.Session.Set<MemberModel.Output.MemberInfo>("Member", anonumousLogin);

            //var message = @"<div style='margin:4px 0;font-family:Arial,Helvetica,sans-serif;font-size:14px;color:#444;line-height:18px;font-weight:normal'>Hi <b>" + request.Name + "</b>,<br/><br/></div>" +
            //            "<div>Thank you for registering as a member at NStore.<br/><br/> </div>" +
            //            "<div>Your account:<br/><br/> </div>" +
            //            "<div>UserName: " + anonumousLogin.Email + ".<br/><br/> </div>" +
            //            "<div>Password: " + anonumousLogin.Password + ".<br/><br/> </div>";

            //var sendMail = Utilities.SendMail("Confirm register member", message, request.Email);

            return Ok();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new MemberModel.Output.Register();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(MemberModel.Output.Register request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = Utilities.SendDataRequest<MemberModel.Output.MemberInfo>(ConstantValues.Member.Register, request);

            if (result.Name == null)
            {
                ModelState.AddModelError("", result.Noteti);
                return View();
            }

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(request.Email));
            //Tạo link xác nhận kích hoạt tài khoản thành viên
            var callbackUrl = Url.ActionLink("ConfirmEmail", "Member",
                                            new { area = "", code = code }, Request.Scheme);
            //Tạo nội dung Email
            var message = @"<div style='margin:4px 0;font-family:Arial,Helvetica,sans-serif;font-size:14px;color:#444;line-height:18px;font-weight:normal'>Hi <b>" + request.Name + "</b>,<br/><br/></div>" +
                        "<div>Thank you for registering as a member at NStore.<br/><br/> </div>" +
                        "<div>To complete the registration, please <b><a style='text-decoration: none;' href='" + callbackUrl + "'><span style='background-color: #ff6600 !important; color: #ffffff; padding: 5px 10px; border-radius: 3px;'>Click here</span></a></b><br/><br/></div>";
            var sendMail = Utilities.SendMail("Confirm register member", message, request.Email);

            if (sendMail == true)
            {
                TempData["result"] = "You must active your account, please check your email";
                return RedirectToAction("Login", "Member");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            var model = new MemberModel.Input.ConfirmEmail();
            return View(model);
        }

        [HttpPost]
        public IActionResult ForgetPassword(MemberModel.Input.ConfirmEmail input)
        {
            if (!ModelState.IsValid)
                return View(input);
            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Member.CheckEmail, input);
            if (result.Issuccess == false)
            {
                ModelState.AddModelError("", result.Noteti);
                return View();
            }

            return RedirectToAction("ChangePassword", input);
        }

        [HttpGet]
        public IActionResult ChangePassword(MemberModel.Input.ConfirmEmail input)
        {
            var model = new MemberModel.Input.ChangePassword();

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangePassword(MemberModel.Input.ChangePassword input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var changePassword = new MemberModel.Input.ChangePassword()
            {
                Email = input.Email,
                OldPassword = input.OldPassword,
                NewPassword = input.NewPassword
            };

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Member.ChangePassword, changePassword);

            if (result.Issuccess == false)
            {
                ModelState.AddModelError("", result.Noteti);
                return View();
            }

            ViewData["Noteti"] = result.Noteti;
            return View();

            //var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input.Email));
            ////Tạo link xác nhận kích hoạt tài khoản thành viên
            //var callbackUrl = Url.ActionLink("ConfirmEmail", "Member",
            //                                new { area = "", code = code }, Request.Scheme);
            ////Tạo nội dung Email
            //var message = @"<div style='margin:4px 0;font-family:Arial,Helvetica,sans-serif;font-size:14px;color:#444;line-height:18px;font-weight:normal'>Hi <b>""</b>,<br/><br/></div>" +
            //            "<div>Thank you for registering as a member at NStore.<br/><br/> </div>" +
            //            "<div>To complete the registration, please <b><a style='text-decoration: none;' href='" + callbackUrl + "'><span style='background-color: #ff6600 !important; color: #ffffff; padding: 5px 10px; border-radius: 3px;'>Click here</span></a></b><br/><br/></div>";
            //var sendMail = Utilities.SendMail("Confirm register member", message, input.Email);

            //if (sendMail == true)
            //{
            //    TempData["result"] = "You must active your account, please check your email";
            //    return RedirectToAction("Login", "Member");
            //}
        }

        public IActionResult ConfirmEmail(string code)
        {
            var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var input = new MemberModel.Input.ConfirmEmail { Email = email };
            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Member.ConfirmEmail, input);
            if (result.Issuccess == true)
            {
                ViewData["NotetiActive"] = "success";
            }
            else
            {
                ViewData["NotetiActive"] = "fail";
            }
            return View();
        }

        public IActionResult CheckInfo()
        {
            return View();
        }

        private bool LoginUser(MemberModel.Output.MemberInfo member, bool remember)
        {
            try
            {
                var userClaims = new List<Claim>() {
                    new Claim("MemberId", member.Id.ToString()),
                    new Claim("USERNAME", member.Email),
                    new Claim("Role", "member"),
                    new Claim("NAME", member.Name),
                    new Claim("EMAIL", member.Email)
                };
                var claimsIdentity = new ClaimsIdentity(userClaims,
                                            CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);
                var properties = new AuthenticationProperties { IsPersistent = remember };
                HttpContext.SignInAsync(principal, properties);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}