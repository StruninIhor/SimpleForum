using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContract.Models
{
    public class ArticleModel : BaseEntityModel
    {
        public string Name { get; set; }

        public string Text { get; set; }
    }
}
