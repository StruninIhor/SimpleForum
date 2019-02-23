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
        [Display(Name = "Email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Enable email notifications")]
        public bool EmailNotificationsEnabled { get; set; }
        [Required]
        [Display(Name = "Enable forum notifications")]
        public bool ForumNotificationsEnabled { get; set; }
        [Required]
        [Display(Name = "Enable subscription")]
        public bool SubscriptionEnabled { get; set; }
    }
}