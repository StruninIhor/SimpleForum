using DataAccessServices.Models;
using DataAccessServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models.Account;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.AspNet.Identity.Owin;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Account
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var user = await UserService.GetUser(User.Identity.Name);

            if (user == null)
            {
                return View("Error");
            }
            AccountViewModel model = new AccountViewModel
            {
                Email = user.Email,
                RegistrationDate = user.RegistrationDate,
                Status = user.Status,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserService.Create(new User { Email = model.Email, Password = model.Password });

                if (result.Succedeed)
                {
                    return View("Home", "Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await UserService.UserExists(model.Email))
                {
                    var claim = await UserService.Authenticate(new User { Email = model.Email, Password = model.Password });
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, claim);
                    return RedirectToLocal(ViewBag.ReturnUrl);
                }
                ModelState.AddModelError("", "Incorrect login or password");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            var result = await UserService.ConfirmEmailAsync(userId, code);

            if (result.Succedeed)
            {
                return View("ConfirmEmailSucessful");
            }
            else
            {
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmEmail(int userId)
        {
            var code = await UserService.GenerateConfirmationTokenAsync(userId);

            var link = Url.Action("ConfirmEmail", "Account", new
            {
                userId,
                code
            }, protocol: Request.Url.Scheme);

            await UserService.SendConfirmationMessageAsync(userId, link);

            return View("ConfirmEmailLinkSent");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserService.GetUser(model.Email);
                if (user != null)
                {
                    string code = await UserService.GeneratePasswordResetTokenAsync(user.Id);

                    var resetLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, code });

                    await UserService.SendResetPasswordMessageAsync(user.Id, resetLink);

                    return View("ForgotPasswordLinkSent");
                }
                else
                {
                    ModelState.AddModelError("Email", "User was not found");
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public async Task<ActionResult> ResetPassword(string userId, string code)
        {
            var user = await UserService.GetUser(userId);
            ResetPasswordViewModel model = new ResetPasswordViewModel
            {
                Code = code,
                Email = (user != null) ? user.Email : null,
                Password = "",
                PasswordConfirm = ""
            };

            return code == null ? View("Error") : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserService.GetUser(model.Email);
                if (user != null)
                {
                    var result = await UserService.ResetPasswordAsync(user.Id, model.Code, model.Password);
                    if (!result.Succedeed)
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                return View("ResetPasswordSuccessful");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            var user = await UserService.GetUser(User.Identity.Name);

            if (user == null)
            {
                return View("Error");
            }

            EditProfileViewModel model = new EditProfileViewModel
            {
                Email = user.Email,
                Status = user.Status,
                UserName = user.UserName,
                EmailNotificationsEnabled = user.EmailNotificationsEnabled,
                ForumNotificationsEnabled = user.ForumNotificationsEnabled,
                SubscriptionEnabled = user.SubscriptionEnabled,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserService.GetUser(model.Email);
                if (user == null)
                {
                    return View("Error");
                }

                if (UserService.Authenticate(new DataAccessServices.Models.User { Email = model.Email, Password = model.OldPassword}) != null)
                {
                    var result =  await UserService.ResetPasswordAsync(user.Id,
                    await UserService.GeneratePasswordResetTokenAsync(user.Id), model.NewPassword);

                    if (result.Succedeed)
                    {
                        return View("ChangePasswordSuccessful");
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Incorrect password");
                }
            }
            return View(model);
        }
        //To be continued...
        [HttpPost]
        public JsonResult GetUsers()
        {
            return Json(UserService.GetUsers());
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}