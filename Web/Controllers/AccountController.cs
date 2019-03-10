using BusinessContract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models.Account;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Configuration;
using BusinessContract.Models;

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

        bool init = false;

        async Task initAdmin()
        {
            if (!init)
            {
                await UserService.SetInitialData(new BusinessContract.Models.User
                {
                    Email = WebConfigurationManager.AppSettings["AdminEmail"],
                    Password = WebConfigurationManager.AppSettings["AdminPassword"],
                    RegistrationDate = DateTime.Now,
                    Status = "Vi veri universum vivus vici",
                    UserName = "admin",
                    EmailNotificationsEnabled = false,
                    ForumNotificationsEnabled = false,
                    SubscriptionEnabled = false
                },
            new List<string> { "user", "admin", "superadmin", "moderator" });

                init = true;
            }
        }
        // GET: Account
        [Authorize]
        public async Task<ActionResult> Index()
        {
            await initAdmin();

            var user = await UserService.GetUser(User.Identity.Name);

            if (user == null)
            {
                return View("Error");
            }
            AccountViewModel model = new AccountViewModel
            {
                Id = user.Id,
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
            await initAdmin();
            if (ModelState.IsValid)
            {
                var result = await UserService.Create(new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    Status = model.Status,
                    UserName = model.UserName,
                    EmailNotificationsEnabled = model.EmailNotificationsEnabled,
                    ForumNotificationsEnabled = model.ForumNotificationsEnabled,
                    SubscriptionEnabled = model.SubscriptionEnabled
                });

                if (result.Succedeed)
                {
                    return View("SuccessfulRegistration");
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
            await initAdmin();
            if (ModelState.IsValid)
            {
                if (await UserService.UserExists(model.Email))
                {
                    var claim = await UserService.Authenticate(new User { Email = model.Email, Password = model.Password });
                    if (claim != null)
                    {

                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe
                        }, claim);
                        return RedirectToLocal(ViewBag.ReturnUrl);
                    }
                }
                ModelState.AddModelError("", "Incorrect login or password");
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserProfile(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                var user = await UserService.GetUser((int)id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                //TODO 
                throw new NotImplementedException();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            try
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
            catch
            {
                return View("Error");
            }
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmEmail(int Id)
        {
            var code = await UserService.GenerateConfirmationTokenAsync(Id);

            var link = Url.Action("ConfirmEmail", "Account", new
            {
                userId = Id,
                code
            }, protocol: Request.Url.Scheme);

            await UserService.SendConfirmationMessageAsync(Id, link);

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
                    string code = UserService.GeneratePasswordResetToken(user.Id);

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
        [HttpPost]
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
        public async Task<ActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserService.GetUser(model.Email);

                if (user == null)
                {
                    return View("Error");
                }

                user.EmailNotificationsEnabled = model.EmailNotificationsEnabled;
                user.ForumNotificationsEnabled = model.ForumNotificationsEnabled;
                user.Status = model.Status;
                user.SubscriptionEnabled = model.SubscriptionEnabled;
                user.UserName = model.UserName;

                var result = await UserService.Update(user);

                if (result.Succedeed)
                {
                    return RedirectToAction("EditProfile");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserService.GetUser(User.Identity.Name);
                if (user == null)
                {
                    return View("Error");
                }

                if (UserService.Authenticate(new BusinessContract.Models.User { Email = user.Email, Password = model.Password}) != null)
                {
                    var result =  await UserService.ResetPasswordAsync(user.Id,
                    UserService.GeneratePasswordResetToken(user.Id), model.NewPassword);

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
                    ModelState.AddModelError("Password", "Incorrect password");
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