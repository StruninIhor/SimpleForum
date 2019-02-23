﻿using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public ActionResult GetForums()
        {
            return Json(new { data = forumService.GetForums() });
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
         // [ValidateAntiForgeryToken]
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
                return View(forum);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult EditForum(DataAccessServices.Models.ForumModel model)
        {
            if (ModelState.IsValid)
            {
                var result = forumService.Update(model);
                if (result.Succedeed)
                {
                    return RedirectToAction("GetForum", new { id = model.Id});
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult DeleteForum(int id)
        {
            var result = forumService.Delete(id);

            if (result.Succedeed)
            {
                return View("Index");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult AddTopic(int forumId)
        {
            var model = new AddTopicViewModel{ ForumId = forumId, Force = false};
            ViewBag.ForceMessage = false;
            return View(model);
        }

        [HttpPost]
         // [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<ActionResult> AddTopic(AddTopicViewModel model)
        {
            ViewBag.ForceMessage = false;
            if (ModelState.IsValid)
            {
                var result = await forumService.AddTopic(model.ForumId, model.Name, model.Text, User.Identity.Name);

                if (result.Succedeed)
                {
                    return RedirectToAction("GetForum", new { id = model.ForumId });
                }
                else if (result.Property == "name")
                {
                    ViewBag.ForceMessage = true;
                    ModelState.AddModelError("", "There is another topic with this name");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }
    }
}