using System;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst_Practice.Models
{
    public class ApplicationDBContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=DemoTest;user=sa;Password=Password123@jkl#");
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Comment> Comments { get; set; }

    }
}
