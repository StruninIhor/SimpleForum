using DataContract.Identity;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        AppRoleManager RoleManager { get; }
        IProfileManager ProfileManager { get; }
        IRepository<Forum> Forums { get; }
        IRepository<Article> Articles { get; }
        IRepository<Topic> Topics { get; }
        IRepository<Comment> Comments { get; }
        void Save();
        Task SaveAsync();
    }
}
