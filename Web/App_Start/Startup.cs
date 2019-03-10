using BusinessContract;
using DependencyInjection;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web.Configuration;
using Web.IoC;

[assembly: OwinStartup(typeof(Web.App_Start.Startup))]

namespace Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.CreatePerOwinContext<IUserService>(KernelHolderSingleton.KernelHolder.CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }
}
