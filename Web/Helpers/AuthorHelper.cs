using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Util;

namespace Web.Helpers
{
    public static class AuthorHelper
    {
        private class Author
        {
            public int Id { get; set; }
            public string ProfileUrl { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
        }

        public static IUserService userService = KernelHolder.CreateUserService();

        public static IHtmlString GetAuthor(BaseEntityModel model, string UserProfileUrl)
        {
            var author = AuthorFromId(model.AuthorId, UserProfileUrl);
            return new HtmlString($"Created {model.CreatedDate.ToString()} <br />by <a href=\"{author.ProfileUrl}\">{author.UserName}<a />");
        }

        private static Author AuthorFromId(int id, string UserProfileUrl)
        {
            var user = userService.GetUserById(id);
            string href = string.Empty;
            string userName;

            if (user != null)
            {
                userName = user.UserName;
                href = UserProfileUrl + $"/{user.Id}";
            }
            else
            {
                userName = "Deleted account";
                href = UserProfileUrl;
            }
            return new Author
            {
                Id = id,
                ProfileUrl = UserProfileUrl,
                UserName = userName
            };
            }
        
        public static IHtmlString GetCommentAuthor(CommentModel comment, string UserUrl)
        {
            var user = AuthorFromId(comment.AuthorId, UserUrl);

            return new HtmlString($"{comment.CreatedDate.ToShortDateString()}<br> <a href=\"{user.ProfileUrl}\">{user.UserName}<a />");
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