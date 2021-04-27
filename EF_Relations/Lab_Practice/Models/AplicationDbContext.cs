using System;
using Microsoft.EntityFrameworkCore;

namespace Lab_Practice.Models
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext()
        { }

        public AplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<EmployeeClub> EmployeeClubs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=EfCoreDemo;user=sa;Password=Password123@jkl#");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  // Fluent API used for detailed setting of all the prop related to the classes and their relation. 
        {                                                                   // For more accurate and detailed DB structure, different from the default EF settings.

            modelBuilder.Entity<Department>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);


            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());   // Employee configuration 


            /*
             * All of the below code can be in another class
             */

            ////modelBuilder
            ////    .Entity<Employee>()      // Selecting which entity/obj/table we will modify
            ////    .ToTable("People", "company");    // Changing Table Name and dbo. Name. Even that the name is changed we are still going to use the class original name to operate with the db from VS. 


            //modelBuilder.Entity<Employee>()     // Changing prop/column name. 
            //    .Property(x => x.StartWorkDate)
            //    .HasColumnName("StartedOn");
            ////.HasColumnType("date");  // The column data type change should be avoided, because if we choose different DB, it might not be supported. 


            ////modelBuilder.Entity<Employee>()   // Assigning new PK 
            ////    .HasKey(x => x.EID);


            //modelBuilder.Entity<Employee>()    // making Egn required AKA not null in order to be part of the composite key 
            //    .Property(x => x.Egn)
            //    .IsRequired();

            //modelBuilder.Entity<Employee>()  // Making EID to be IDENTITY
            //    .Property(x => x.EID)
            //    .UseIdentityColumn();

            //modelBuilder.Entity<Employee>()        // this will create a composite key from EID which is the PK and Egn
            //    .HasKey(x => new { x.EID, x.Egn });

            //modelBuilder.Entity<Employee>()    // Making that prop/column required/not null
            //    .Property(x => x.FirstName)
            //    .IsRequired();

            //modelBuilder.Entity<Employee>()
            //    .Property(x => x.FirstName)
            //    .HasMaxLength(30);


            //modelBuilder.Entity<Employee>()   // NOT NULL
            //    .Property(x => x.LastName)
            //    .IsRequired()
            //    .HasMaxLength(30);


            //modelBuilder.Entity<Employee>()   // Will ignore the prop/column after created, will not show in the DB.
            //    .Ignore(x => x.FullName);


            ////modelBuilder.Entity<Employee>()   // This will ignore the input thru VS and will set the value to the default one. 
            ////    .Property(x => x.Salary)
            ////    .ValueGeneratedOnUpdate();


            //modelBuilder.Entity<Employee>()             // Describing (one to many)
            //    .HasOne(x => x.Department)              // Employee can have only one Dep 
            //    .WithMany(x => x.Employees)             // Department can have many employees 
            //    .HasForeignKey(x => x.DepartmentId)     // 
            //    .OnDelete(DeleteBehavior.Restrict);     // Will not allow cascade deleting by default => By default will delete Departments and Employees (Cascade)



            //modelBuilder.Entity<Address>()      // Setting One to One Employee <=> Address
            //    .HasOne(a => a.Employee)
            //    .WithOne(e => e.Address)
            //    .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Employee>()     // One to Many relation between Employees and Departments
            //    .HasOne(e => e.Department)              // 1 => Employee to have a Department  (must)
            //    .WithMany(d => d.Employees)             // 2 => Department to have a Collection of Employee   (easier for LINQ queries) (optional, good practice)
            //    .HasForeignKey(e => e.DepartmentId)     // 3 => Employee to have DepartmentId as a physical column and FK to Departments (optional, good practice)
            //    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EmployeeClub>()           // Creating composite key for the mapping table
                 .HasKey(x => new { x.ClubId, x.EmployeeId });


            {
                base.OnModelCreating(modelBuilder);
            }

        }
    }
}