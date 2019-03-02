using DataAccessServices.Infrastructure;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Interfaces
{
    public interface ITopicService : IDisposable
    {
        Task<OperationDetails> Create(int forumId, string name, string text, string authorEmail);
        OperationDetails Update(TopicModel item);
        OperationDetails Delete(int id);

        TopicModel GetById(int id, bool includeComments = true);

        ICollection<TopicModel> GetTopics(bool includeComments = true);

        int TopicsCount();
    }
}
