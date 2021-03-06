﻿using BusinessContract;
using BusinessContract.Infrastructure;
using BusinessContract.Models;
using DataContract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class ArticleService : IArticleService
    {
        public IUnitOfWork Database;

        public ArticleService(IUnitOfWork unit)
        {
            Database = unit;
        }

        private bool ContainSripts(string someString)
        {
            if (someString.Contains("href=\"javascript:\""))
                return true;
            else return false;
        }

        public async Task<OperationDetails> Create(string name, string text, string authorEmail)
        {
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);

            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }
            if (ContainSripts(text))
            {
                return new OperationDetails(false, "Don't user scripts in article text!", "");
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

        public ArticleModel GetArticle(int id)
        {
            return Mapper.Map(Database.Articles.GetById(id));
        }

        public OperationDetails Update(ArticleModel item)
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

            if (ContainSripts(item.Text))
            {
                return new OperationDetails(false, "Don't user scripts in article text!", "");
            }

            article.Name = item.Name;
            article.Text = item.Text;
            Database.Articles.Update(article);
            Database.Save();
            return new OperationDetails(true, "Article was updated sucessfully", "");
        }
    }
}
