using System.Collections.Generic;

namespace DataContract.Models
{
    public class Forum : BaseEntity
    {
        public Forum()
        {
            Topics = new List<Topic>();
        }

        public string Name { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }
}