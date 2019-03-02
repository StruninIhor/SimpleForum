using DataContract.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Models
{
    public class UserProfile
    {
        
        public int Id { get; set; }

        public int Rating { get; set; }

        public virtual AppUser AppUser { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }


        public bool EmailNotificationsEnabled { get; set; }
        public bool ForumNotificationsEnabled { get; set; }
        public bool SubscriptionEnabled { get; set; }
    }
}
