using BusinessContract.Infrastructure;
using BusinessContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContract
{
    public interface IForumService : IDisposable
    {
        Task<OperationDetails> Create(string name, string authorEmail);
        OperationDetails Update(ForumModel item);
        OperationDetails Delete(int id);
        ForumModel GetForum(int id);

        ICollection<ForumModel> GetForums();
    }
}
