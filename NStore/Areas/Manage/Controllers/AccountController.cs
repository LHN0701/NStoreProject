using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NStore.Areas.Manage.Models.Authorization;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        [Authorize("Admin", "Staff")]
        public IActionResult Index()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }

            var listUser = Utilities.SendDataRequest<List<UserModel.UserBase>>(ConstantValues.User.GetAllUser);

            var id = User.Claims.FirstOrDefault(x => x.Type == "USERID").Value;

            var user = listUser.Where(x => x.Id.Equals(int.Parse(id))).ToList();
            if (user.FirstOrDefault(x => x.Id.Equals(int.Parse(id))).Role == "Admin")
            {
                ViewData["Role"] = true;
                return View(listUser);
            }
            else
            {
                ViewData["Role"] = false;
                return View(user);
            }
        }

        [HttpGet]
        [Authorize("Admin", "Staff")]
        public IActionResult Detail()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "USERID").Value;
            var user = Utilities.SendDataRequest<UserModel.UserBase>(ConstantValues.User.GetById + $"/{id}", id);
            return View(user);
        }

        [HttpGet]
        [Authorize("Admin", "Staff")]
        public IActionResult ChangePassword()
        {
            UserModel.Input.ChangePassword model = new();
            return View(model);
        }

        [HttpPost]
        [Authorize("Admin", "Staff")]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var identity = User.Claims.FirstOrDefault(x => x.Type == "IDENTIFICATION").Value;
            var id = User.Claims.FirstOrDefault(x => x.Type == "USERID").Value;

            var input = new UserModel.Input.ChangePasswordInfo
            {
                Id = int.Parse(id),
                Username = identity,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.User.ChangePassword, input);

            if (result.Issuccess)
                ViewData["Noteti"] = $"<span style='color: #0000ff;'>{result.Noteti}</span>";
            else
            {
                ViewData["Noteti"] = $"<span style='color: #ff0000;'>{result.Noteti}</span>";
            }

            return View();
        }

        [HttpGet]
        [Authorize("Admin", "Staff")]
        public IActionResult Create()
        {
            UserModel.UserBase model = new();
            return View(model);
        }

        [HttpPost]
        [Authorize("Admin", "Staff")]
        public IActionResult Create(UserModel.UserBase input)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.User.CreateUser, input);
            if (result.Issuccess)
            {
                TempData["result"] = result.Noteti;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Noteti);
            return View();
        }

        [HttpGet]
        [Authorize("Admin", "Staff")]
        public IActionResult Update(int id)
        {
            if (id > 0)
            {
                var user = Utilities.SendDataRequest<UserModel.UserBase>(ConstantValues.User.GetById + $"/{id}", id);
                if (user != null && user.Id > 0)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize("Admin", "Staff")]
        public IActionResult Update(UserModel.UserBase input)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.User.UpdateUser + $"/{input.Id}", input);

            if (result.Issuccess)
            {
                TempData["result"] = result.Noteti;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Noteti);
            return View();
        }

        [HttpGet]
        [Authorize("Admin", "Staff")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.User.DeleteUser + $"/{id}", id);

            if (result.Issuccess)
            {
                TempData["result"] = result.Noteti;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Noteti);
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            LoginModel model = new();
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

            var input = new UserModel.Input.LoginInfo { UserName = username, Password = password };
            var user = Utilities.SendDataRequest<UserModel.Output.UserInfo>(ConstantValues.User.Login, input);
            HttpContext.Session.Remove("User");
            if (user != null)
            {
                if (user.Id > 0)
                {
                    bool logined = LoginUser(user, remember);
                    if (logined)
                        HttpContext.Session.Set<UserModel.Output.UserInfo>("User", user);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", user.Noteti);
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            //var user = HttpContext.Session.Get<UserModel.Output.UserInfo>("NhanVien");
            //var input = new UserModel.Input.LoginInfo { UserName = user.Identification, Password = user.Password };
            var input = @User.Claims.FirstOrDefault(x => x.Type == "NAME").Value;
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            Utilities.SendDataRequest<bool>(ConstantValues.User.Logout, input);
            return RedirectToAction("Login", "Account");
        }

        private bool LoginUser(UserModel.Output.UserInfo user, bool remember)
        {
            try
            {
                var userClaims = new List<Claim>() {
                    new Claim("USERID", user.Id.ToString()),
                    new Claim("USERNAME", user.Identification),
                    new Claim("ROLE", user.Role.ToUpper()),
                    new Claim("NAME", user.Name),
                    new Claim("IDENTIFICATION", user.Identification)
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}