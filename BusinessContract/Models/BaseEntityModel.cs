using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContract.Models
{
    public class BaseEntityModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDateString { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
