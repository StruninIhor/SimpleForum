﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Models
{
    public class CommentModel : BaseEntityModel
    {
        public int TopicId { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int? ReplyToCommentId { get; set; }
        public ICollection<CommentModel> Replies { get; set; }

    }
}
