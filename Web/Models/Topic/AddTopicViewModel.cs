using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Topic
{
    public class AddTopicViewModel
    {
        [Required]
        public int ForumId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Name { get; set; }

    }
}