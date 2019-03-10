using BusinessContract;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public class KernelHolder
    {
        public KernelHolder(string connectionString, string smtpServer, string email, string password)
        {
            NinjectModule module = new ForumModule(connectionString, smtpServer, email, password);
            Kernel = new StandardKernel(module);
        }

        public StandardKernel Kernel { get; protected set; }

        public IUserService CreateUserService()
        {
            return Kernel.Get<IUserService>();
        }
    }
}
