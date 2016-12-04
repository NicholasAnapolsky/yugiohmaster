using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Milestone_3.Models
{
    public class Cart
    {
        public Cart()
        {

        }
        public Cart(string Name, string Color, Decimal? Price, string Size, Decimal? Weight)
        {
            this.Name = Name;
            this.Color = Color;
            this.Price = Price;
            this.Size = Size;
            this.Weight = Weight;
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public Decimal? Price { get; set; }
        public string Size { get; set; }
        public Decimal? Weight { get; set; } 
    }
}