using FluentValidation;
using FluentValidation.Attributes;
using Milestone_3.Models;
using MileStone2A.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Milestone_3.Models
{
    [Table("dbo.Reviews")]
    public class Reviews
    {
        [Key]
        public int id { get; set; }

        public String Name { get; set; }
        
        public int ProductID { get; set; }

        public String Review { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating my be between 1 and 5 inclusive")]
        public int Rating { get; set; }

    }
}