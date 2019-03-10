using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContract.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }

        public int Rating { get; set; }

        public DateTime RegistrationDate { get; set; }
        public string UserName { get; set; }

        public string Status { get; set; }

        public bool EmailNotificationsEnabled { get; set; }
        public bool ForumNotificationsEnabled { get; set; }
        public bool SubscriptionEnabled { get; set; }
    }
}
