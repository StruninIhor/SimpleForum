using DataContract.Identity;
using DataContract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationContext Database { get; set; }

        AppUserManager userManager;
        AppRoleManager roleManager;
        IClientManager clientManager;

        public UnitOfWork(string connectionString)
        {
            Database = new ApplicationContext(connectionString);
            userManager = new AppUserManager(new CustomUserStore(Database));
            roleManager = new AppRoleManager(new CustomRoleStore(Database));
            clientManager = new ClientManager(Database);
        }

        public AppUserManager UserManager => userManager;

        public AppRoleManager RoleManager => roleManager;

        public IClientManager ClientManager => clientManager;

        public void Save()
        {
            Database.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Database.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    //clientManager
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
