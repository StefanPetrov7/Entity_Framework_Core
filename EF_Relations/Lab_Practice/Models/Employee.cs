using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab_Practice.Models
{
    //[Table("People", Schema = "company")]  // => changing table and schema name
    //[Index("Egn",  "Salary")]  // Adding indexes to specific columns one or many
    //[Index("Egn", IsUnique = true)] // Adding  uniqueness to that index
    //[Index("Egn", IsUnique = true, Name = "IX_Egn")] // adding name to the index
    public class Employee
    {
        public Employee()
        {
            this.EmployeeClub = new HashSet<EmployeeClub>();
            this.Employees = new HashSet<Employee>();
        }

        [Key]  // will mark it as PK, !! Composite Key can be made only thru FLUENT API 
        public int Id { get; set; }   // By default will create the Id as PK, Identity. It should always be named as Id so EF core in order to be assigned correctly.
                                      // If name is not 'Id' the new name can be set from OnModelCreating.
                                       

       //[Column("EGN", Order = 2, TypeName = "NVARCHAR(30)")]   // changing column name, order number, data type
        public string Egn { get; set; }


      //[Required]  // NOT NULL
      //[MaxLength(20)] // Max length
        public string FirstName { get; set; }  // Default NVARCHAR(MAX)



        public string LastName { get; set; } // Default NVARCHAR(MAX)

      //[NotMapped] // will ignore the column
        public string FullName => this.FirstName + " " + this.LastName;



        public DateTime? StartWorkDate { get; set; }  //  '?' => will allow the DateTime to have null values, by default it cannot be null



        public decimal Salary { get; set; }  // Deafult cannot be null decimal(18,2)





        // Optional => highly recommended.
        public int DepartmentId { get; set; }   // This column is auto created in DB one Department has been set as reference, but we should add it here as well. 

        [ForeignKey(nameof(DepartmentId))]  // will describe the FK column related to the navigational prop
        [InverseProperty("Employees")] // Showing the relation to the inverse prop
        public Department Department { get; set; }     // Creating a realation to the Department (one to many)




        [ForeignKey("Address")]
        public int? AddressId { get; set; }  // one to one with address // allowing null in order the employee to be inserted without an address


        public Address Address { get; set; }   // one to one with address



        public ICollection<EmployeeClub> EmployeeClub { get; set; }   // mapping table collection


        public int? BirthTownId { get; set; }

        [InverseProperty(nameof(Town.Locals))]
        public Town BirthTown { get; set; }




        public int? WorkTownId { get; set; }

        [InverseProperty(nameof(Town.Workers))]
        public Town WorkTown { get; set; }


        // Self Reference ===>

        public int? ManagerId { get; set; }    // set to ? allow null in order not all the employees to have manager

        public Employee Manager { get; set; }

        [InverseProperty(nameof(Manager))]
        public ICollection<Employee> Employees { get; set; }

    }
}

