using BusinessContract;
using BusinessContract.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models.Comment;

namespace Web.Controllers
{
    public class CommentController : Controller
    {
        ICommentService commentService;
        ITopicService topicService;
        IUserService userService;
        public CommentController(ICommentService commentService, ITopicService topicService, IUserService userService)
        {
            this.commentService = commentService;
            this.topicService = topicService;
            this.userService = userService;
        }

        [HttpGet]
        public JsonResult Comment(int topicId, bool recursive = true)
        {
            var topic = topicService.GetById(topicId, includeComments: false);
            if (topic == null)
            {
                return NotFound("Topic not found");
            }
            var comments = commentService.GetTopicComments(topicId);
            if (!recursive)
            {
                comments = comments.Where(c => c.Order == 0).ToList();
            }
            return GetJson(comments);
        }

        [HttpGet]
        public JsonResult GetReplies(int commentId)
        {
            var comments = commentService.GetReplies(commentId);

            if (comments != null)
            {
                return GetJson(comments);
            }
            else
            {
                return NotFound("Comment was not found");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddComment(CommentViewModel model)
        {
            if (topicService.GetById(model.TopicId) == null)
            {
                return NotFound("Topic was not found");
            }
            OperationDetails result;
            if (model.ReplyToCommentId != null)
            {
                if (commentService.GetById((int)model.ReplyToCommentId) == null)
                {
                    return NotFound("ReplyTo comment was not found");
                }
                result = await commentService.ReplyTo((int)model.ReplyToCommentId, model.Text, User.Identity.Name);
            }
            else
            {
                result = await commentService.Create(model.TopicId, model.Text, User.Identity.Name);
            }
            if (result.Succedeed)
            {
                return GetJson(commentService.GetTopicComments(model.TopicId));
            }
            else
            {
                return ServerError(result.Message);
            }
                        
        }

        [HttpPost]
        [Authorize]
        public JsonResult Comment(EditCommentViewModel model)
        {
            var comment = commentService.GetById(model.Id);

            if (comment == null)
            {
                return NotFound("Comment to update was not found");
            }

            var result = commentService.Update(comment.Id, model.Text);

            if (result.Succedeed)
            {
                return GetJson(commentService.GetTopicComments(model.TopicId));
            }
            else
            {
                return ServerError(result.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public JsonResult Comment(DeleteCommentViewModel model)
        {
            var comment = commentService.GetById(model.Id);

            if (comment == null)
            {
                return NotFound("Comment to delete not found");
            }
            //If author is deleting his comment, or some of admin trying to delete it
            if (User.IsInRole("admin") || 
                User.IsInRole("superadmin") || 
                User.IsInRole("moderator") || 
                userService.GetUserById(model.AuthorId) == userService.GetUserByEmail(User.Identity.Name))
            {
                var result = commentService.Delete(model.Id);
                if (result.Succedeed)
                {
                    return GetJson(commentService.GetTopicComments(model.TopicId));
                }
                return ServerError(result.Message);
            }
            else
            {
                Response.StatusCode = 401;
                return GetJson(new { message = "Unauthorized" });
            }
        }

        JsonResult GetJson(object data) => Json(data, JsonRequestBehavior.AllowGet);

        JsonResult NotFound(string message = "Resource not found")
        {
            Response.StatusCode = 404;
            Response.StatusDescription = message;
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }

        JsonResult ServerError(string message = "Internal server error")
        {
            Response.StatusCode = 500;
            Response.StatusDescription = message;
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
    }
}