using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models.Forum;

namespace Web.Controllers
{
    public class ForumController : Controller
    {
        IForumService forumService;

        public ForumController(IForumService fs)
        {
            forumService = fs;
        }

        private JsonResult GetJson(object data) => Json(data, JsonRequestBehavior.AllowGet);

        private JsonResult NotFound(string message = "Resource not found")
        {
            Response.StatusCode = 404;
            Response.StatusDescription = message;
            return GetJson(new { message });
        }

        [HttpGet]
        public ActionResult GetForums()
        {
            //Imitation if loading
            Thread.Sleep(1000);
            return Json(new { data = forumService.GetForums() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTopics(int forumId)
        {
            var forum = forumService.GetForum(forumId);

            if (forum == null)
            {
                return NotFound("Forum not found");
            }

            return GetJson(forum.Topics);
        }

        // GET: Forum
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="admin, superadmin")]
         [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await forumService.Create(model.Name, User.Identity.Name);
                if (result.Succedeed)
                {
                    return View("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetForum(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var forum = forumService.GetForum((int)id);
                if (forum==null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(forum);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles="admin, superadmin")]
        public ActionResult EditForum(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var forum = forumService.GetForum((int)id);
            if (forum == null)
                return HttpNotFound();
            else
            {
                return View(new EditForumViewModel {Name = forum.Name, Id = forum.Id});
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, superadmin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditForum(EditForumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var forum = forumService.GetForum(model.Id);
                if (forum!=null)
                {
                    forum.Name = model.Name;
                    var result = forumService.Update(forum);
                    if (result.Succedeed)
                    {
                        return RedirectToAction("GetForum", new { id = model.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Forum was not found");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="superadmin")]
        public ActionResult DeleteForum(int id)
        {
            return View(new DeleteForumViewModel { Id = id});
        }

        [HttpPost]
        [Authorize(Roles = "superadmin")]
        [ActionName("DeleteForum")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForumConfirmed(DeleteForumViewModel model)
        {
            var result = forumService.Delete(model.Id);

            if (result.Succedeed)
            {
                return View("Index");
            }
            else
            {
                return View("Error");
            }
        }

    }
}