using DataAccessServices.Interfaces;
using DataAccessServices.Modules;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Web.Util
{
    public static class KernelHolder
    {
        static StandardKernel kernel;

        public static StandardKernel Kernel
        {
            get
            {
                if (kernel == null)
                {
                    NinjectModule serviceModule = new ServiceModule("DefaultConnection", WebConfigurationManager.AppSettings["SmtpServer"], 
                        WebConfigurationManager.AppSettings["EmailAddress"],
                        WebConfigurationManager.AppSettings["Password"]);
                    //TODO Create ForumModule
                    kernel = new StandardKernel(serviceModule);
                }
                return kernel;
            }
        }

        public static IUserService CreateUserService()
        {
            return KernelHolder.Kernel.Get<IUserService>();
        }
    }
}