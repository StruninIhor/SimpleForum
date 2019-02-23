using DataContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Interfaces
{
    public interface IRepository<T> where T: BaseEntity
    {
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Func<T, bool> predicate);
    }
}
