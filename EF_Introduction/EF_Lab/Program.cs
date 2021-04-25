using System;
using EF_Lab.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EF_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            //var options = new DbContextOptionsBuilder<SoftUniContext>()    // changing db
            //    .UseSqlServer("Server=localhost;Database=SoftUni;user=sa;Password=Password123@jkl#");
            //var dbContext = new SoftUniContext(options.Options);


            var dbContext = new SoftUniContext();

            Console.WriteLine(dbContext.Employees.Count());

            // CRUD

            var employee = new Employee
            {
                FirstName = "Lu",
                LastName = "BinBin",
                MiddleName ="L",
                JobTitle = "PES",
                DepartmentId = 1,
                ManagerId=1,
                AddressId=1,
                HireDate = new DateTime(2000, 04, 15),
                Salary = 100000
            };

            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();

            // CRUD

            //var topDepartments = dbContext.Departments.OrderByDescending(x => x.Employees.Count())
            //    .Select(x => new { x.Name, x.Employees.Count })
            //    .ToList();

            ////Console.WriteLine(topDepartments.ToQueryString()); // Query built until is still IQueryable

            //foreach (var em in topDepartments)
            //{
            //    Console.WriteLine($"{em.Name} - {em.Count}");
            //}
        }
    }
}
