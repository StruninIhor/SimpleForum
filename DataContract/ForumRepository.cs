using DataContract.Interfaces;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class ForumRepository : IRepository<Forum>
    {
        ApplicationContext db;

        public ForumRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Create(Forum item)
        {
            db.Forums.Add(item);   
        }

        public void Delete(int id)
        {
            var item = db.Forums.Find(id);
            if (item != null)
            {
                db.Forums.Remove(item);
            }
        }

        public IEnumerable<Forum> GetAll()
        {
            return db.Forums.Select(f => f);
        }

        public Forum GetById(int id)
        {
            var result = db.Forums.Find(id);
            return result;
        }

        public void Update(Forum item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<Forum> Where(Func<Forum, bool> predicate)
        {
            return db.Forums.Where(predicate).Select(f => f);
        }
    }
}
