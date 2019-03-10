using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Models.DataAnnotations;

namespace Web.Models.Article
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(1500, MinimumLength = 28)]
        [AllowHtml, RemoveScript]
        public string Text { get; set; }
    }
}