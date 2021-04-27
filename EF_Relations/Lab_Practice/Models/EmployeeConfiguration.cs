using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lab_Practice.Models
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>   // All the modelBuilder options to Entity Employee can be in a separate class
    {

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //modelBuilder
            //    .Entity<Employee>()      // Selecting which entity/obj/table we will modify
            //    .ToTable("People", "company");    // Changing Table Name and dbo. Name. Even that the name is changed we are still going to use the class original name to operate with the db from VS. 


            builder         // Changing prop/column name. 
                   .Property(x => x.StartWorkDate)
                   .HasColumnName("StartedOn");
            //.HasColumnType("date");  // The column data type change should be avoided, because if we choose different DB, it might not be supported. 


            //modelBuilder.Entity<Employee>()   // Assigning new PK 
            //    .HasKey(x => x.EID);


            builder         // making Egn required AKA not null in order to be part of the composite key 
                 .Property(x => x.Egn)
                 .IsRequired();

            //builder          // Making EID to be IDENTITY
            //    .Property(x => x.EID)
            //    .UseIdentityColumn();

            //builder              // this will create a composite key from EID which is the PK and Egn
            //    .HasKey(x => new { x.EID, x.Egn });

            builder               // Making that prop/column required/not null
                .Property(x => x.FirstName)
                .IsRequired();

            builder
                 .Property(x => x.FirstName)
                 .HasMaxLength(30);


            builder                 // NOT NULL
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(30);


            builder                 // Will ignore the prop/column after created, will not show in the DB.
                .Ignore(x => x.FullName);


            //builder             // This will ignore the input thru VS and will set the value to the default one. 
            //    .Property(x => x.Salary)
            //    .ValueGeneratedOnUpdate();


            builder           // Describing (one to many)
                 .HasOne(x => x.Department)              // Employee can have only one Dep 
                 .WithMany(x => x.Employees)             // Department can have many employees 
                 .HasForeignKey(x => x.DepartmentId)     // 
                 .OnDelete(DeleteBehavior.Restrict);     // Will not allow cascade del


            builder
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(f => f.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
