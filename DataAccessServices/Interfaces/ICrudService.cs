using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Interfaces
{
    public interface ICrudService<T> :IDisposable
        where T:BaseEntityModel
    {
        void Add(T item);
        void GetById(int id);
        void Update(T item);
        void Delete(T item);

        ICollection<T> Find(Func<bool, T> expression);
    }
}
