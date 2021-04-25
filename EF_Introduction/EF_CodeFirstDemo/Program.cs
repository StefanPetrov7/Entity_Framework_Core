using System;
using EF_CodeFirstDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_CodeFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SliDoDbContext();
            db.Database.Migrate();  // This will enable migrations
        }
    }
}
