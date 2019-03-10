using BusinessContract;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models.Article;

namespace Web.Controllers
{
    public class ArticleController : Controller
    { 
        IArticleService articleService;
        IUserService userService;

        public ArticleController(IArticleService articleService, IUserService userService)
        {
            this.articleService = articleService;
            this.userService = userService;
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
            return View();
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
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Name", result.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
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

            if (User.IsInRole("user"))
            {
                if (article.AuthorId != (await userService.GetUser(User.Identity.Name)).Id)
                {
                    return RedirectToAction("Login");
                }
            }

            EditArticleViewModel model = new EditArticleViewModel
            {
                Id = article.Id,
                Name = article.Name,
                Text = article.Text
            };
            return View(model);
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

        [HttpGet]
        public JsonResult GetArticles()
        {
            var articles = articleService.GetArticles();

            return Json(articles, JsonRequestBehavior.AllowGet);
        }
        
    }
}