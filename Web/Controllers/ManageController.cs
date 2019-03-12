using BusinessContract;
using BusinessContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public JsonResult GetUsers()
        {
            ICollection<User> users = userService.Users;
            return GetJson(new { data = users});
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            var user = userService.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            else return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = userService.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                var result = await userService.Delete(id);
                if (result.Succedeed)
                {
                    return GetJson(new { status = "OK", message = result.Message });
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.StatusDescription = result.Message;
                    return GetJson(new { status = "Bad request", message = result.Message });
                }
            }
        }

        JsonResult GetJson(object data) => Json(data, JsonRequestBehavior.AllowGet);
    }
}