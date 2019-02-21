using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class EditProfileViewModel
    {
        [Editable(false)]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public bool EmailNotificationsEnabled { get; set; }
        [Required]
        public bool ForumNotificationsEnabled { get; set; }
        [Required]
        public bool SubscriptionEnabled { get; set; }
    }
}