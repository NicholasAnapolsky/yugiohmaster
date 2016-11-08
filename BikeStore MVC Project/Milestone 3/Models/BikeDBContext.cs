using Milestone_3.Models;
using MileStone2A.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MileStone2A.Models
{
    public class BikeDBContext : DbContext
    {
        public BikeDBContext(): base("Adventure")
        {
        }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductAndDescription2> ProductAndDescription { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductModel> ProductModel { get; set; }
    }
}