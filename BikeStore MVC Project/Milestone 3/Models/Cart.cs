using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milestone_3.Models
{
    public class Cart
    {
        public Cart()
        {

        }
        public Cart(int? productID, string Name, string Color, Decimal? Price, string Size, Decimal? Weight)
        {
            this.productID = productID;
            this.Name = Name;
            this.Color = Color;
            this.Price = Price;
            this.Size = Size;
            this.Weight = Weight;
        }

        [Key]
        public int? productID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Decimal? Price { get; set; }
        public string Size { get; set; }
        public Decimal? Weight { get; set; } 
    }
}