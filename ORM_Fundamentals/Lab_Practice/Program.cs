using System;
using System.Linq;
using Lab_Practice.Models;
using System.Collections.Generic;

namespace Lab_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            SoftUniContext db = new SoftUniContext();

            var employees = db.Employees.ToList();

            foreach (var emmployee in employees)
            {
                emmployee.Salary *= 1.10m;
            }

            db.SaveChanges();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.Salary}");
            }

        }
    }
}
