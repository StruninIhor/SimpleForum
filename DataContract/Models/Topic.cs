using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Models
{
    public class Topic : BaseEntity
    {
        public Topic()
        {
            Comments = new List<Comment>();
        }

        public int ForumId { get; set; }
        public Forum Forum { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
