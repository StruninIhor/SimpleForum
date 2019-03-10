using BusinessContract;
using BusinessContract.Infrastructure;
using BusinessContract.Models;
using DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class CommentService : ICommentService
    {
        public IUnitOfWork Database;

        public CommentService(IUnitOfWork db)
        {
            Database = db;
        }

        public async Task<OperationDetails> Create(int topicId, string text, string authorEmail)
        {
            var topic = Database.Topics.GetById(topicId);
            if (topic == null)
            {
                return new OperationDetails(false, "Topic was not found", "");
            }
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);
            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }
            Database.Comments.Create(new DataContract.Models.Comment
            {
                Author = user,
                CreatedDate = DateTime.Now,
                Order = 0,
                Text = text,
                Topic = topic
            });
            await Database.SaveAsync();
            return new OperationDetails(true, "Comment was successfully added", "");
        }

        public OperationDetails Delete(int commentId)
        {
            var comment = Database.Comments.GetById(commentId);
            if (comment == null)
            {
                return new OperationDetails(false, "Comment was not found", "");
            }
            Database.Comments.Delete(commentId);
            Database.Save();
            return new OperationDetails(true, "Comment was successfully deleted", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public CommentModel GetById(int commentId)
        {
            return Mapper.Map(Database.Comments.GetById(commentId), false);
        }

        public ICollection<CommentModel> GetReplies(int commentId, bool recursive = false)
        {
            var comment = Database.Comments.GetById(commentId);
            if (comment == null)
            {
                return null;
            }
            ICollection<CommentModel> result = new List<CommentModel>();
            foreach (var reply in comment.Replies)
            {
                result.Add(Mapper.Map(reply, false));
            }
            return result;
        }

        public async Task<OperationDetails> ReplyTo(int replyToId, string text, string authorEmail)
        {
            var replyTo = Database.Comments.GetById(replyToId);
            if (replyTo == null)
            {
                return new OperationDetails(false, "Reply to comment was not found", "");
            }
            var user = await Database.UserManager.FindByEmailAsync(authorEmail);
            if (user == null)
            {
                return new OperationDetails(false, "Author was not found", "");
            }
            replyTo.Replies.Add(new DataContract.Models.Comment
            {
                Text = text,
                Author = user,
                ReplyTo = replyTo,
                CreatedDate = DateTime.Now,
                Order = replyTo.Order + 1,
                Topic = replyTo.Topic
            });
            await Database.SaveAsync();
            return new OperationDetails(true, "Reply was successfully added", "");
        }

        public OperationDetails Update(int commentId, string newText)
        {
            var comment = Database.Comments.GetById(commentId);
            if (comment == null)
            {
                return new OperationDetails(false, "Comment was not found", "");
            }
            comment.Text = newText;
            Database.Comments.Update(comment);
            Database.Save();
            return new OperationDetails(true, "Comment was updated successfuly", "");
        }

        public ICollection<CommentModel> GetTopicComments(int topicId)
        {
            var topic = Database.Topics.GetById(topicId);

            if (topic != null)
            {
                return Mapper.Map(topic, true).Comments;
            }
            else
            {
                return null;
            }
        }
}
}
