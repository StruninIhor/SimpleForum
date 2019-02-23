using DataAccessServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IForumService forumService;
        IArticleService articleService;
        public HomeController(IForumService fs, IArticleService articleService)
        {
            forumService = fs;
            this.articleService = articleService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        string Path(string item) => $"~/App_Data/{item}";

        [HttpPost]
        public JsonResult GetMenu()
        {
            string listIcon = Path("List.ico");
            var forumsItem = new MenuItem { Name = "Forums", Order = 0, Icon = listIcon };
            var articlesItem = new MenuItem { Name = "Articles", Order = 0, Icon = listIcon };
            ICollection<MenuItem> items = new List<MenuItem>
                {forumsItem, articlesItem};
            foreach (var forum in forumService.GetForums())
            {
                var item = new MenuItem { Order = 1, Icon = Path("Forum.ico"), Name = forum.Name };
                forumsItem.Children.Add(item);

                foreach (var topic in forumService.GetForum(forum.Id).Topics)
                {
                    forumsItem.Children.Add(new MenuItem { Order = forumsItem.Order + 1, Name = topic.Name, Icon = Path("Topic.ico") });
                }
            }
            foreach (var article in articleService.GetArticles())
            {
                articlesItem.Children.Add(new MenuItem { Order = 1, Name = article.Name, Icon = Path("Article.ico") });
            }
            return Json(new { data = items });
        }

    }
}