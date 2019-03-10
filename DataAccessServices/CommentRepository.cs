using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataContract
{
    public class CommentRepository : IRepository<Comment>
    {
        public ApplicationContext db;

        public CommentRepository(ApplicationContext ctx)
        {
            db = ctx;
        }

        public void Create(Comment item)
        {
            db.Comments.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Comments.Find(id);
            if (item != null)
            {
                db.Comments.Remove(item);
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            return db.Comments/*.Select(a => a)*/;
        }

        public Comment GetById(int id)
        {
            return db.Comments.Find(id);
        }

        public IEnumerable<TResult> Select<TResult>(Func<Comment, TResult> expression)
        {
            return db.Comments.Select(expression);
        }

        public void Update(Comment item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<Comment> Where(Func<Comment, bool> predicate)
        {
            return db.Comments.Where(predicate).Select(a => a);
        }
    }
}
