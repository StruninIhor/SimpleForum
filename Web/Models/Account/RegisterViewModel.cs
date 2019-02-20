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
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}