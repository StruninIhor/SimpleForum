using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }

        public string Status { get; set; }

        public int Rating { get; set; }

        public int ArticlesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}