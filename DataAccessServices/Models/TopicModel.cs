using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Models
{
    public class TopicModel : BaseEntityModel
    {
        public int ForumId { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public ICollection<CommentModel> Comments { get; set; }
    }
}
