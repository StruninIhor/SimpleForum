using DataContract.Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Identity
{
    public class ApplicationContext : IdentityDbContext<AppUser, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationContext(string connectionString) : base(connectionString) { }
        public ApplicationContext() : this("DefaultConnection") { }
    }
}
