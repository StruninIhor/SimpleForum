using DataContract.Interfaces;
using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class TopicRepository : IRepository<Topic>
    {
        public ApplicationContext db;

        public TopicRepository(ApplicationContext ctx)
        {
            db = ctx;
        }

        public void Create(Topic item)
        {
            db.Topics.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Topics.Find(id);
            if (item != null)
            {
                db.Topics.Remove(item);
            }
        }

        public IEnumerable<Topic> GetAll()
        {
            return db.Topics.Select(a => a);
        }

        public Topic GetById(int id)
        {
            return db.Topics.Find(id);
        }

        public void Update(Topic item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<Topic> Where(Func<Topic, bool> predicate)
        {
            return db.Topics.Where(predicate).Select(a => a);
        }
    }
}
