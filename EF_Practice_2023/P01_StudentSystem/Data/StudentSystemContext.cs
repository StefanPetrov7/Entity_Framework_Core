using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.ModelBuilding;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        { }

        public StudentSystemContext(DbContextOptions option) : base(option)
        { }

        public DbSet<Student>? Students { get; set; }

        public DbSet<Homework>? Homeworks { get; set; }

        public DbSet<Course>? Courses { get; set; }

        public DbSet<Resource>? Resources { get; set; }

        public DbSet<StudentCourse>? StudentsCourses { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=True;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());

            modelBuilder.ApplyConfiguration(new CourseConfiguration());

            modelBuilder.ApplyConfiguration(new ResourceConfiguration());

            modelBuilder.ApplyConfiguration(new HomeworkConfiguration());

            modelBuilder.ApplyConfiguration(new StudentCourseConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
