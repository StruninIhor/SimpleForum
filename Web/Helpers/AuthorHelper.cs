using BusinessContract;
using BusinessContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.IoC;

namespace Web.Helpers
{
    public static class AuthorHelper
    {
        public static IUserService userService = KernelHolderSingleton.KernelHolder.CreateUserService();

        public static IHtmlString GetAuthor(BaseEntityModel model, string UserProfileUrl)
        {
            var user = userService.GetUserById(model.AuthorId);
            string href = string.Empty;
            string userName;
            if (user != null)
            {
                userName = user.UserName;
                href = UserProfileUrl + $"?id={user.Id}";
            }
            else
            {
                userName = "Deleted account";
                href = UserProfileUrl;
            }

            return new HtmlString($"Created {model.CreatedDate.ToString()} <br />by <a href=\"{href}\">{userName}<a />");
        }

        public static int GetUserIdByEmail(string email)
        {
            var user = userService.GetUserByEmail(email);

            if (user == null)
            {
                return -1;
            }
            else
            {
                return user.Id;
            }
        }
    }
}