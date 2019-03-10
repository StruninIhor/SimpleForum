using DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Web.IoC
{
    public class KernelHolderSingleton
    {
        private KernelHolderSingleton()
        {

        }

        static KernelHolder kernelHolder;

        public static KernelHolder KernelHolder
        {
            get
            {
                if (kernelHolder == null)
                {
                    kernelHolder = new KernelHolder(WebConfigurationManager.AppSettings["ConnectionString"], WebConfigurationManager.AppSettings["SmtpServer"],
                        WebConfigurationManager.AppSettings["EmailAddress"],
                        WebConfigurationManager.AppSettings["Password"]);
                }
                return kernelHolder;
            }
        }
    }
}