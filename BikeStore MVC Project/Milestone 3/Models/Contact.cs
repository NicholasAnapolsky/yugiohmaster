using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milestone_3.Models
{
    public class Contact
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-z1-9-_]*@[a-zA-z1-9-_]*\\.[a-z]*$", ErrorMessage = "The Email must be a valid email.")]
        public string Email { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}