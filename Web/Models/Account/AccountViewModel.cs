using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class AccountViewModel
    {
        public string Email { get; set; }
        
        public DateTime RegistrationDate { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}