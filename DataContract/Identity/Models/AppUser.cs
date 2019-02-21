using DataContract.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Identity.Models
{
    public class AppUser : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
    UserManager<AppUser, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }

        public bool IsBlocked { get; set; }

        public virtual UserProfile Profile { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Forum> Forums { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
