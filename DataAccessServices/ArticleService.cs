using DataAccessServices.Infrastructure;
using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using DataContract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices
{
    public class ArticleService : IArticleService
    {
        public IUnitOfWork Database;

        public ArticleService(IUnitOfWork unit)
        {
            Database = unit;
        }

        public async Task<OperationDetails> Create(string name, string text, string authorEmail, bool force = false)
        {
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);

            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }
            var article = Database.Articles.Where(a => a.Name == name);

            if (article != null && !force)
            {
                return new OperationDetails(false, "There is another topic with the same name", "name");
            }
            Database.Articles.Create(new DataContract.Models.Article
            {
                Author = user,
                CreatedDate = DateTime.Now,
                Name = name,
                Text = text
            });
            await Database.SaveAsync();
            return new OperationDetails(true, "Article was added successfully", "");
        }

        public OperationDetails Delete(int id)
        {
            var article = Database.Articles.GetById(id);

            if (article == null)
            {
                return new OperationDetails(false, "Article was not found", "");
            }
            Database.Articles.Delete(id);
            Database.Save();
            return new OperationDetails(true, "Article was successfully deleted", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public ICollection<ArticleModel> GetArticles()
        {
            ICollection<ArticleModel> result = new List<ArticleModel>();

            foreach (var article in Database.Articles.GetAll())
            {
                result.Add(Mapper.Map(article));
            }
            return result;
        }

        public TopicModel GetTopic(int id)
        {
            return Mapper.Map(Database.Topics.GetById(id));
        }

        public OperationDetails Update(TopicModel item)
        {
            var article = Database.Articles.GetById(item.Id);
            if (article == null)
            {
                return new OperationDetails(false, "Article was not found", "");
            }
            if (article.AuthorId != item.AuthorId)
            {
                return new OperationDetails(false, "Wrong author", "");
            }
            article.Name = item.Name;
            article.Text = item.Text;
            Database.Articles.Update(article);
            Database.Save();
            return new OperationDetails(true, "Article was updated sucessfully", "");
        }
    }
}
