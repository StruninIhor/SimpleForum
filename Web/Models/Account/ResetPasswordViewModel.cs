using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }
    }
}