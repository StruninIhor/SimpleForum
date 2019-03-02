using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models.Topic;

namespace Web.Controllers
{
    public class TopicController : Controller
    {
        ITopicService topicService;
        IForumService forumService;
        ICommentService commentService;
        public TopicController(ITopicService topicService, IForumService forumService, ICommentService commentService)
        {
            this.topicService = topicService;
            this.forumService = forumService;
            this.commentService = commentService;
        }

        // GET: Topic
        // Returns random topic
        public ActionResult Index()
        {
            Random random = new Random();
            int rnd = random.Next(topicService.TopicsCount());
            return RedirectToAction("Topic", new { id = rnd });   
        }

        [HttpGet]
        public ActionResult Topic(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var topic = topicService.GetById((int)id);

                if (topic == null)
                {
                    return HttpNotFound();
                }

                return View(topic);
            }
        }


        [HttpGet]
        [Authorize(Roles = "moderator, admin, superadmin")]
        public ActionResult Create(int forumId)
        {
            return View(new AddTopicViewModel { ForumId = forumId});
        }

        [HttpPost]
        [Authorize(Roles = "moderator, admin, superadmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddTopicViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await topicService.Create(model.ForumId, model.Name, model.Text, User.Identity.Name);

                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public PartialViewResult GetCommentForm(int topicId)
        {
            var topic = topicService.GetById(topicId);

            if (topic == null)
            {
                return BadRequest("Topic was not found");
            }

            return PartialView("_GetCommentForm");
        }

        [ChildActionOnly]
        [Authorize]
        public PartialViewResult GetComments(TopicModel model)
        {
            if (model!=null)
            {
                var comments = model.Comments;
                return PartialView("_GetComments", model);
            }
            return BadRequest("Topic was not found");
            
        }

        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Comment(CommentViewModel model)
        {
            try
            {
                var result = await commentService.Create(model.TopicId, model.Text, User.Identity.Name);

                if (result.Succedeed)
                {
                    var comments = commentService.GetTopicComments(model.TopicId);
                    if (comments != null)
                    {
                        return PartialView($"_GetComments/{model.TopicId}");
                    }
                    else throw new Exception("Topic was not found");
                }
                throw new Exception(result.Errors.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public PartialViewResult GetReplyForm(int commentId)
        {
            var comment = commentService.GetById(commentId);

            if (comment == null)
            {
                return BadRequest("Comment to reply was not found");
            }

            return PartialView();
        }


        [HttpGet]
        public 
        #region Helpers

        PartialViewResult BadRequest(string status = "An error occured while processing your request")
        {
            //Debugger.Launch();
            //Debugger.Break();
            Response.StatusCode = 400;
            //Response.Status = status;
            Response.StatusDescription = status;
            return PartialView("_Error");
        }

        #endregion

    }
}