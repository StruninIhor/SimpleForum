using DataContract;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessServices
{
    public class ArticleRepository : IRepository<Article>
    {
        public ApplicationContext db;

        public ArticleRepository(ApplicationContext ctx)
        {
            db = ctx;
        }

        public void Create(Article item)
        {
            db.Articles.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Articles.Find(id);
            if (item != null)
            {
                db.Articles.Remove(item);
            }
        }

        public IEnumerable<Article> GetAll()
        {
            return db.Articles;
        }

        public Article GetById(int id)
        {
            return db.Articles.Find(id);
        }

        public IEnumerable<TResult> Select<TResult>(Func<Article, TResult> expression)
        {
            return db.Articles.Select<Article, TResult>(expression);
        }

        public void Update(Article item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<Article> Where(Func<Article, bool> predicate)
        {
            return db.Articles.Where(predicate).Select(a => a);
        }
    }
}
