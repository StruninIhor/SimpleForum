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

        public async Task<OperationDetails> Comment(int topicId, string authorEmail, string text, int? replyToCommentId)
        {
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);

            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }

            var topic = Database.Topics.GetById(topicId);

            if (topic == null)
            {
                return new OperationDetails(false, "Topic was not found", "");
            }
            var replyTo = new DataContract.Models.Comment();
            if (replyToCommentId != null)
            {
                replyTo = Database.Comments.GetById((int)replyToCommentId);
                if (replyTo == null)
                {
                    return new OperationDetails(false, "Reply to comment was not found", "");
                }
                if (replyTo.TopicId != topicId)
                {
                    return new OperationDetails(false, "Reply to comment topic and reply topic must be the same", "");
                }
                replyTo.Replies.Add(new DataContract.Models.Comment
                {
                    Author = user,
                    Topic = topic,
                    CreatedDate = DateTime.Now,
                    Order = replyTo.Order + 1,
                    Text = text
                });
                return new OperationDetails(true, "Reply was added", "");
            }
            else
            {
                topic.Comments.Add(new DataContract.Models.Comment
                {
                    Order = 0,
                    CreatedDate = DateTime.Now,
                    Author = user,
                    ReplyTo = null,
                    Text = text,
                    Topic = topic
                });
                return new OperationDetails(true, "Comment was added", "");
            }

            
        }

        public async Task<OperationDetails> Create(int forumId, string name, string authorEmail)
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
                Author = user
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

        public TopicModel GetById(int id)
        {
            return Mapper.Map(Database.Topics.GetById(id));
        }

        public ICollection<TopicModel> GetTopics()
        {
            ICollection<TopicModel> result = new List<TopicModel>();
            foreach(var topic in Database.Topics.GetAll())
            {
                result.Add(Mapper.Map(topic));
            }
            return result;
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
