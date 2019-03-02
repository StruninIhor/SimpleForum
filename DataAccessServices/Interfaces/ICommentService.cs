using DataAccessServices.Infrastructure;
using DataAccessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Interfaces
{
    public interface ICommentService : IDisposable
    {
        Task<OperationDetails> Create(int topicId, string text, string authorEmail);
        OperationDetails Update(int commentId, string newText);
        OperationDetails Delete(int commentId);

        Task<OperationDetails> ReplyTo(int replyToId, string text, string authorEmail);

        CommentModel GetById(int commentId);

        ICollection<CommentModel> GetReplies(int commentId, bool recursive = false);

        ICollection<CommentModel> GetTopicComments(int topicId);
    }
}
