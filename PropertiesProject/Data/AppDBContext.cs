using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext()
        { }

        public AppDBContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Models.Property> Properties { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<BuildingType> BuildingTypes { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Tag> Tags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(SqlConfiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}