using System;
using System.Linq;
using Lab_Practice.Models;

namespace Lab_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new AplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var department = new Department { Name = "HR" };

            var footbalClub = new Club { Name = "FootbalClub" };
            var swimmingClub = new Club { Name = "Swimming " };

            db.Clubs.Add(footbalClub);
            db.Clubs.Add(swimmingClub);



            for (int i = 0; i < 10; i++)
            {
                var employee = new Employee
                {
                    Egn = "1221324" + i,
                    FirstName = "Niki_" + i,
                    LastName = "Kostov",
                    StartWorkDate = new DateTime(2010 + i, 1, 1),
                    Salary = 100 + i,
                    Department = department,
                };

                db.Employees.Add(employee);
            }

            

            db.SaveChanges();
        }
    }
}
