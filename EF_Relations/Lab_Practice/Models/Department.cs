using System;
using System.Collections.Generic;

namespace Lab_Practice.Models
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>(); // Inverse prop collection
        }



        public int Id { get; set; }



        public string Name { get; set; }


        // Optional => highly recommended.
        public ICollection<Employee> Employees { get; set; }    // Inverse prop, used for better queries with LINQ. Not required but highly recommended. 
                                                                // Coming from the relation (one to many) Employee (DepartmentId) => Departments (PK Id) 



    }               
}
