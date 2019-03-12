using BusinessContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "admin, superadmin")]
    public class ManageController : Controller
    {
        IUserService userService;


        public ManageController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }


    }
}