using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Models
{
    public class Article : BaseEntity
    {
        public string Name { get; set; }

        public string Text { get; set; }
    }
}
