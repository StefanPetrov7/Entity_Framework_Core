using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FastFood.Data
{
    public class FastFoodContextDesignTimeFactory : IDesignTimeDbContextFactory<FastFoodContext>
    {
        public FastFoodContextDesignTimeFactory()
        {
        }

        public FastFoodContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FastFoodContext>();
            builder.UseSqlServer("Server=.;Database=FastFood;user=sa;Password=Password123@jkl#;MultipleActiveResultSets=true");

            return new FastFoodContext(builder.Options);
        }
    }
}
