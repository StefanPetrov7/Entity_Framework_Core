using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

            var dbContext = new SoftUniContext();

            Console.WriteLine(DeleteProjectById(dbContext));

        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townToRemove = context.Towns
                .Include(x => x.Addresses)
                .FirstOrDefault(x => x.Name == "Seattle");   // getting the town that will be removed, and the mapping table in order to obtain the related id' to the employees

            var allAddressId = townToRemove.Addresses.Select(x => x.AddressId).ToList(); // getting all addresses with that town, related to the employees 

            var employeeId = context.Employees                                      // getting all the employees Id with that address.
                .Where(x => x.AddressId.HasValue && allAddressId.Contains(x.AddressId.Value))
                .ToList();

            foreach (var address in employeeId)            // setting the employees address to null 
            {
                address.AddressId = null;
            }

            foreach (var addressId in allAddressId)      // delete the addresses 
            {
                var addressToRemove = context.Addresses.FirstOrDefault(x => x.AddressId == addressId);
                context.Addresses.Remove(addressToRemove);
            }

            context.Towns.Remove(townToRemove);          // delete the town 

            context.SaveChanges();

            string result = $"{allAddressId.Count} addresses in Seattle were deleted";

            return result;

        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var prjToRemove = context.Projects     // taking the prj together with the mapping table.
                .Include(x => x.EmployeesProjects)
                .FirstOrDefault(x => x.ProjectId == 2);

            var empIdconnectedTpPrjId = prjToRemove.EmployeesProjects.Select(x => x.EmployeeId).ToList();  // finding all the empId related to that prj.

            foreach (var id in empIdconnectedTpPrjId)
            {
                var idToRemove = context.EmployeesProjects.FirstOrDefault(x => x.EmployeeId == id);     // cast the id to correct type
                context.EmployeesProjects.Remove(idToRemove);             // removing the empID related to that prj 
            }

            context.Projects.Remove(prjToRemove);    // deleting the prj

            context.SaveChanges();

            var projects = context.Projects.Select(x => new { x.Name }).Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var prj in projects)
            {
                sb.AppendLine($"{prj.Name}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => EF.Functions.Like(x.FirstName, "sa%"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary,
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)  // TODO as Judge kill's it with Zero!!!! 
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .ThenBy(x => x.Name)  // TODO lexicographically sorting need to be fixed.
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.StartDate,
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var prj in projects)
            {
                sb.AppendLine($"{prj.Name}");
                sb.AppendLine($"{prj.Description}");
                sb.AppendLine($"{prj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employees = context.Employees
                .Where(x => departments.Contains(x.Department.Name))
                .ToList();

            foreach (var emp in employees)
            {
                emp.Salary *= 1.12M;
            }

            context.SaveChanges();

            var ordered = employees.Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.Salary,

            }).ToList()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in ordered)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    x.Manager.FirstName,
                    x.Manager.LastName,
                    employees = x.Employees
                    .Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.JobTitle,
                    })
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()
                });

            StringBuilder sb = new StringBuilder();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.Name} - {dep.FirstName} {dep.LastName}");

                foreach (var emp in dep.employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    projects = x.EmployeesProjects
                .Select(x => new { x.Project.Name })
                .OrderBy(x => x.Name)
                .ToList()       // => When sorting anonymous obj. 
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employee)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");

                foreach (var prj in emp.projects)
                {
                    sb.AppendLine($"{prj.Name}");
                }
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(x => x.Employees.Count())
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Select(x => new
                {
                    x.AddressText,
                    x.Town.Name,
                    count = x.Employees.Count(),
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var adr in addresses)
            {
                sb.AppendLine($"{adr.AddressText}, {adr.Name} - {adr.count} employees");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects
                .Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    MgrFirstName = x.Manager.FirstName,
                    MgrLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects
                    .Select(x => new
                    {
                        x.Project.Name,
                        x.Project.StartDate,
                        x.Project.EndDate

                    })
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.MgrFirstName} {emp.MgrLastName}");

                foreach (var prj in emp.Projects)
                {
                    var endDate = prj.EndDate.HasValue ?
                          prj.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";

                    sb.AppendLine($"--{prj.Name} - {prj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();

        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            //var newAddress = new Address
            //{
            //    AddressText = "Vitoshka 15",
            //    TownId = 4,
            //};

            //context.Addresses.Add(newAddress);

            //context.SaveChanges();

            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            //nakov.AddressId = newAddress.AddressId;

            nakov.Address = new Address   // Directly adding the new address which will automatically add it into the address table.
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            context.SaveChanges();

            var query = context.Employees
                .Select(x => new { x.Address.AddressText, x.Address.AddressId })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in query)
            {
                sb.AppendLine($"{emp.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var query = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Salary,
                    DepName = x.Department.Name,   // => Renaming prop 

                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            foreach (var emp in query)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} from {emp.DepName} - ${emp.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var query = context.Employees
                .OrderBy(x => x.EmployeeId)
                .Select(x => new { x.FirstName, x.LastName, x.MiddleName, x.JobTitle, x.Salary })
                .ToList();

            foreach (var emp in query)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var query = context.Employees.Where(x => x.Salary > 50_000)
                .Select(x => new { x.FirstName, x.Salary })
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var emp in query)
            {
                sb.AppendLine($"{emp.FirstName} - {emp.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
