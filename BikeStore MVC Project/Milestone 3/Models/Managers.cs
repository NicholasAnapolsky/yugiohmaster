using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Milestone_3.Models
{
    public class Managers
    {
        [Key]
        public int ManagerID { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        //[Remote("ExistingEmail", "Home", ErrorMessage = "This Email does not exist, please use an existing one.")]
        [Required]
        public string Email { get; set; }
        //[Remote("ExistingPassword", "Home", ErrorMessage = "This Password does not exist, please use an existing one.")]
        [Required]
        public string Password { get; set; }
    }
}

/*
 * CREATE TABLE [dbo].[Managers] (
    [ManagerID] INT          IDENTITY (1, 1) NOT NULL,
    [LastName]  VARCHAR (30) NOT NULL,
    [FirstName] VARCHAR (30) NOT NULL,
    [Email]     VARCHAR (50) NOT NULL,
    [Password]  VARCHAR (50) NOT NULL
);
 */
