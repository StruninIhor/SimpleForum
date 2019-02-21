using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Models
{
    public class Comment : BaseEntity
    {
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int? ReplyToCommentId { get; set; }
        public virtual Comment ReplyTo { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }
    }
}
