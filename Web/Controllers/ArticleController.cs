using DataAccessServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models.Article;

namespace Web.Controllers
{
    public class ArticleController : Controller
    { 
        IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        // GET: Article
        public ActionResult Index()
        {
            return View(articleService.GetArticles());
        }

        [HttpGet]
        public ActionResult Article(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Index");
            }
             var article = articleService.GetArticle((int)id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);       
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View(new CreateArticleViewModel { Force = false });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(CreateArticleViewModel model)
        {
            ViewBag.ForceMessage = false;
            if (ModelState.IsValid)
            {
                var result = await articleService.Create(model.Name, model.Text, User.Identity.Name);

                if (result.Succedeed)
                {
                    return View("Index");
                }
                else if (result.Property == "name")
                {
                    if (model.Force)
                    {
                        var forcedResult = await articleService.Create(model.Name, model.Text, User.Identity.Name, true);
                        if (result.Succedeed)
                        {
                            return View("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", forcedResult.Message);
                        }
                    }
                    else
                    {
                        ViewBag.ForceMessage = true;
                        ModelState.AddModelError("Name", result.Message);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "superadmin, admin, moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var article = articleService.GetArticle((int)id);

                if (article == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    EditArticleViewModel model = new EditArticleViewModel
                    {
                        Id = article.Id,
                        Name = article.Name,
                        Text = article.Text
                    };
                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = articleService.GetArticle(model.Id);
                if (article == null)
                {
                    ModelState.AddModelError("", "Article was not found");
                }
                else
                {
                    article.Name = model.Name;
                    article.Text = model.Text;
                    var result = articleService.Update(article);
                    if (result.Succedeed)
                    {
                        article = articleService.GetArticle(model.Id);
                        return View("Article", article);
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin, supeardmin, moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var article = articleService.GetArticle((int)id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(new DeleteArticleViewModel { Id = article.Id, Name = article.Name });
        }

        [HttpPost]
        [Authorize(Roles = "admin, supeardmin, moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = articleService.Delete(model.Id);

                if (result.Succedeed)
                {
                    return View("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }
        
    }
}