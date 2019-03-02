using DataAccessServices.Infrastructure;
using DataAccessServices.Interfaces;
using DataAccessServices.Models;
using DataContract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices
{
    public class TopicService : ITopicService
    {
        IUnitOfWork Database;

        public TopicService(IUnitOfWork unit)
        {
            Database = unit;
        }

        public async Task<OperationDetails> Create(int forumId, string name, string text, string authorEmail)
        {
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);

            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }
            var forum = Database.Forums.GetById(forumId);

            if (forum == null)
            {
                return new OperationDetails(false, "Forum was not found", "");
            }

            Database.Topics.Create(new DataContract.Models.Topic
            {
                Name = name,
                Forum = forum,
                Author = user,
                CreatedDate = DateTime.Now,
                Text = text
                
            });
            await Database.SaveAsync();
            return new OperationDetails(true, "Topic was created successfully", "");
        }

        public OperationDetails Delete(int id)
        {
            var topic = Database.Topics.GetById(id);

            if (topic == null)
            {
                return new OperationDetails(false, "Topic was not found", "");
            }

            Database.Topics.Delete(id);
            Database.Save();
            return new OperationDetails(true, "Topic was deleted succesffully", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public TopicModel GetById(int id, bool includeComments = true)
        {
            return Mapper.Map(Database.Topics.GetById(id), includeComments);
        }

        public ICollection<TopicModel> GetTopics(bool includeComments = true)
        {
            ICollection<TopicModel> result = new List<TopicModel>();
            foreach(var topic in Database.Topics.GetAll())
            {
                result.Add(Mapper.Map(topic, includeComments));
            }
            return result;
        }

        public int TopicsCount()
        {
            return Database.Topics.Select(t => t.Id).Count();
        }

        public OperationDetails Update(TopicModel item)
        {
            var topic = Database.Topics.GetById(item.Id);

            if (topic == null)
            {
                return new OperationDetails(false, "Topic was not found", "");
            }

            if (topic.AuthorId != item.AuthorId)
            {
                return new OperationDetails(false, "Wrong author", "");
            }
            if (topic.ForumId != item.ForumId)
            {
                return new OperationDetails(false, "Wrong forum", "");
            }
            topic.Name = item.Name;
            topic.Text = item.Text;

            Database.Topics.Update(topic);
            Database.Save();
            return new OperationDetails(true, "Topic was updated sucesfully", "");
        }
    }
}
