using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class MenuItem
    {
        public MenuItem()
        {
            Children = new List<MenuItem>();
        }

        public int Order { get; set; }

        public string Name { get; set; }

        public ICollection<MenuItem> Children { get; set; }

        public string Icon { get; set; }
    }
}