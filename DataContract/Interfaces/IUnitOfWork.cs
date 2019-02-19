using DataContract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        AppRoleManager RoleManager { get; }
        void Save();
        Task SaveAsync();
    }
}
