using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessage = "Email address is incorrect")]
        [Required]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }

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