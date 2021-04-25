using System;
using Microsoft.EntityFrameworkCore;

namespace EF_CodeFirstDemo.Models
{
    public class SliDoDbContext : DbContext
    {
        public SliDoDbContext()
        { }

        /* In case we want to add options from outside.
         * We add empty ctor and want with the option injected.
         * We can use it for changing the connection address.
         * */

        public SliDoDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)  // In case no other configuration has been added or changed 
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=SliDo;user=sa;Password=Password123@jkl#");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().Property(x => x.Content).IsUnicode(false);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
