using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Email is confirmed")]
        public bool EmailConfirmed { get; set; }
    }
}