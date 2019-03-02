using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //After using that, I got @Url.Action("GetUserProfile", "Account") was returned ~/User/{id from model on the page}/{id of corresponding user}
            //routes.MapRoute(
            //    name: "User",
            //    url: "User/{id}",
            //    defaults: new { controller = "Account", action = "GetUserProfile", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
