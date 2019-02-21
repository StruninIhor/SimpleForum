using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessServices.Models
{
    public class ForumModel : BaseEntityModel
    {
        public string Name { get; set; }
        public ICollection<TopicModel> Topics { get; set; }
    }
}
