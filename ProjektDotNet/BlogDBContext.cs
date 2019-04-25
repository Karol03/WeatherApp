using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ProjektDotNet
{
    public class CityDB
    {
        [Key]
        public String Name { get; set; }
        public String Clouds { get; set; }
        public String Temps { get; set; }
        public String Humidity { get; set; }
    }

    public class BlogDBContext : DbContext
    {
        public DbSet<CityDB> Cities { get; set; }
    }
}
