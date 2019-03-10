using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.DataAnnotations;

namespace Web.Models.Comment
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            Replies = new List<CommentViewModel>();
        }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public int Id { get; set; }
        public int Order { get; set; }
        public ICollection<CommentViewModel> Replies { get; set; }
        public int? ReplyToCommentId { get; set; }
        [AllowHtml, RemoveScript]
        public string Text { get; set; }

        public int TopicId { get; set; }
    }
}