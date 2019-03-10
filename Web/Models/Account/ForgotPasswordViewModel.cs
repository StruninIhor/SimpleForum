using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}