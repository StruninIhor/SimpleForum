using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.DataAnnotations;

namespace Web.Models.Article
{
    public class EditArticleViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(1500, MinimumLength = 28)]
        [AllowHtml, RemoveScript]
        public string Text { get; set; }
    }
}