using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Article
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(1500, MinimumLength = 28)]
        public string Text { get; set; }

        [Required]
        public bool Force { get; set; }
    }
}