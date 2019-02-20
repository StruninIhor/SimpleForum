using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address is incorrect")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;  }
    }
}