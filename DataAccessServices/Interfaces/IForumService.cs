using DataAccessServices.Infrastructure;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Interfaces
{
    public interface IForumService : IDisposable
    {
        Task<OperationDetails> Create(string name, string authorEmail);
        OperationDetails Update(ForumModel item);
        OperationDetails Delete(int id);
        ForumModel GetForum(int id);
        Task<OperationDetails> AddTopic(int forumId, string name, string text, string authorEmail, bool force = false);

        ICollection<ForumModel> GetForums();
    }
}
