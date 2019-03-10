using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Forum
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "Forum name")]
        public string Name { get; set; }
    }
}