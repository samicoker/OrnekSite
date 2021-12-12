using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace OrnekSite.Entity
{
    public class DataContext:DbContext
    {
        public DataContext():base("dataConnection") // veritabanı ile bağlantı kurmak için bu classı oluşturduk
        {
            Database.SetInitializer(new DataInitializer());
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
    }
}