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
            ViewBag.Message = "Developed by Strunin Ihor";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        string Path(string item) => $"~/App_Data/{item}";

        public ActionResult NavigationMenuTest(bool modelBased = true)
        {
            if (modelBased)
            {
                return View(GetMenu());
            }
            return View("NavTestJson");
        }

        ICollection<MenuItem> GetMenu()
        {
            string listIcon = Path("List.ico");
            var forumsItem = new MenuItem { Name = "Forums", Order = 0, Icon = listIcon, Id=Url.Action("Index", "Forum")};
            var articlesItem = new MenuItem { Name = "Articles", Order = 0, Icon = listIcon, Id=Url.Action("Index", "Article") };
            ICollection<MenuItem> items = new List<MenuItem>
                {forumsItem, articlesItem};
            foreach (var forum in forumService.GetForums())
            {
                var item = new MenuItem { Order = 1, Icon = Path("Forum.ico"), Name = forum.Name, Id= Url.Action("GetForum", "Forum", new { id = forum.Id}), /*Parent = forumsItem,*/ ParentId = forumsItem.Id };
                forumsItem.Children.Add(item);

                foreach (var topic in forumService.GetForum(forum.Id).Topics)
                {
                    item.Children.Add(new MenuItem { Order = item.Order + 1, Name = topic.Name, Icon = Path("Topic.ico"), Id = Url.Action("GetTopic", "Topic", new { id = topic.Id }), /*Parent = item,*/ ParentId = item.Id });
                }
            }
            foreach (var article in articleService.GetArticles())
            {
                articlesItem.Children.Add(new MenuItem { Order = 1, Name = article.Name, Icon = Path("Article.ico"), Id = Url.Action("GetArticle", "Article", new { id = article.Id }), /*Parent = articlesItem,*/ ParentId = articlesItem.Id });
            }
            return items;
        }

        [HttpPost]
        public JsonResult GetMenuJson()
        {
            return Json(GetMenu());
        }

    }
}