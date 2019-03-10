using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Topic
{
    public class ReplyViewModel
    {
        [Required]
        public int ReplyToCommentId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}