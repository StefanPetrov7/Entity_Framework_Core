using SoftUni.Data;
using System.Text;
using System.Linq;
using SoftUni.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new SoftUniContext();
            string result = RemoveTown(db);
            Console.WriteLine(result);


        }

        public static string RemoveTown(SoftUniContext db)
        {
            var adresses = db.Towns.Include(x => x.Addresses).FirstOrDefault(x => x.Name == "Seattle");
            var listAddressIds = adresses.Addresses.Select(x => x.AddressId).ToList();

            foreach (var id in listAddressIds)
            {
                var emp = db.Employees.Where(x => x.AddressId == id).FirstOrDefault();

                if (emp != null)
                {
                    emp.AddressId = null;
                }
            }

            int count = listAddressIds.Count;

            foreach (var id in listAddressIds)
            {
                var addressToRemove = adresses.Addresses.FirstOrDefault(x => x.AddressId == id);
                db.Addresses.Remove(addressToRemove);
            }

            var seattle = db.Towns.FirstOrDefault(x => x.Name == "Seattle");
            db.Towns.Remove(seattle);
            db.SaveChanges();
            return $"{count} addresses in Seattle were deleted ";
        }

        public static string DeleteProjectById(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var projectsToRemove = db.Projects         // taking the projects and the mapping table 
                .Include(x => x.EmployeesProjects)
                .FirstOrDefault(x => x.ProjectId == 2);

            var empIdToRemove = projectsToRemove.EmployeesProjects.Select(x => x.EmployeeId).ToList();  // List<int> from the emp id which need to be removed

            foreach (var id in empIdToRemove)
            {
                var setOfEmpPrjIdToRemove = db.EmployeesProjects.FirstOrDefault(x => x.EmployeeId == id);  // extracting the set of emp id and prj id object 
                db.EmployeesProjects.Remove(setOfEmpPrjIdToRemove);  // removing the object (empId,prjId) from the table 
            }

            db.Projects.Remove(projectsToRemove);

            db.SaveChanges();

            foreach (var prj in db.Projects)
            {
                result.AppendLine($"{prj.Name}");
            }

            return result.ToString().TrimEnd();

        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var employees = db.Employees.Where(x => EF.Functions.Like(x.FirstName, "sa%"))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var emp in employees)
            {
                result.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();
            string[] departments = new string[]
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employees = db.Employees.Where(x => departments.Contains(x.Department.Name)).ToList();

            foreach (var emp in employees)
            {
                emp.Salary *= 1.12M;
            }

            db.SaveChanges();

            foreach (var emp in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                result.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return result.ToString().TrimEnd();

            // SQL => UPDATE Employees 
            //        SET Salary *= 1.12
            //        FROM Employees AS e
            //        LEFT JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
            //        WHERE d.[Name] IN('Engineering', 'Tool Design', 'Marketing', 'Information Services')

        }

        public static string GetLatestProjects(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var projects = db.Projects.OrderByDescending(x => x.StartDate).ToList().Take(10);

            foreach (var prj in projects.OrderBy(x => x.Name))
            {
                result.AppendLine($"{prj.Name}");
                result.AppendLine($"{prj.Description}");
                result.AppendLine($"{prj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return result.ToString().TrimEnd();

            // SQL => SELECT x.[Name] , x.[Description]
            //            FROM
            //            (
            //                SELECT TOP(10)[Name], Description
            //                FROM Projects
            //                ORDER BY StartDate DESC
            //            ) AS x
            //        ORDER BY x.[Name]

        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var departmens = db.Departments
                .Include(x => x.Employees)
                .Where(x => x.Employees.Count > 5)
                .Select(x => new
                {
                    x.Name,
                    EmpCount = x.Employees.Count,
                    EmpList = x.Employees.Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.JobTitle,
                        x.Salary,
                    })
                }).ToList();


            foreach (var dep in departmens.OrderBy(x => x.EmpCount).ThenBy(x => x.Name))
            {
                foreach (var mgr in dep.EmpList.OrderByDescending(x => x.Salary).Take(1))
                {
                    result.AppendLine($"{dep.Name} - {mgr.FirstName} {mgr.LastName}");

                    var employees = dep.EmpList.OrderByDescending(x => x.Salary).Skip(1).ToList();

                    foreach (var emp in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                    {
                        result.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                    }
                }
            }

            return result.ToString().TrimEnd();

            // SQL Query => SELECT d.[Name], e.FirstName, e.JobTitle, e.Salary,
            //                     DENSE_RANK() OVER(PARTITION BY d.[Name] ORDER BY Salary DESC)
            //                     FROM Employees AS e
            //                     LEFT JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
            //                     GROUP BY d.Name, e.FirstName, e.JobTitle, e.Salary

        }

        public static string GetEmployee147(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var emp = db.Employees.Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .FirstOrDefault(x => x.EmployeeId == 147);

            var projects = emp.EmployeesProjects.Select(x => x.Project).OrderBy(x => x.Name);

            result.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");

            foreach (var prj in projects)
            {
                result.AppendLine(prj.Name);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();
            var addresses = db.Addresses
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    EmpCount = x.Employees.Count,
                })
            .OrderByDescending(x => x.EmpCount)
            .ThenBy(x => x.TownName)
            .Take(10)
            .ToList();

            foreach (var adr in addresses)
            {
                result.AppendLine($"{adr.AddressText}, {adr.TownName} - {adr.EmpCount} employees");
            }

            return result.ToString().TrimEnd();
        }


        public static string GetEmployeesInPeriod(SoftUniContext db)  // Not giving the points, possible copy string issue!!
        {
            StringBuilder result = new StringBuilder();
            var employees = db.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 &&
                                                        p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmpFirstName = x.FirstName,
                    EmpLastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate,
                    })
                }).Take(10)
                .ToList();

            foreach (var emp in employees)
            {
                result.AppendLine($"{emp.EmpFirstName} {emp.EmpLastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");

                foreach (var prj in emp.Projects)
                {
                    var endDate = prj.EndDate.HasValue ? prj.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";
                    result.AppendLine($"--{prj.Name} - {prj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate} ");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();
            var vitoshkaAdress = new Address();
            vitoshkaAdress.AddressText = "Vitoshka 15";
            vitoshkaAdress.TownId = 4;
            db.Addresses.Add(vitoshkaAdress);
            var nakovEmp = db.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            db.SaveChanges();

            nakovEmp.AddressId = vitoshkaAdress.AddressId;

            db.SaveChanges();


            var query = db.Employees.Select(x => new
            {
                x.AddressId,
                AdressText = x.Address.AddressText,
            }).OrderByDescending(x => x.AddressId)
            .Take(10)
            .ToList();

            foreach (var address in query)
            {
                result.AppendLine(address.AdressText);
            }

            return result.ToString().TrimEnd();

        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var query = db.Employees.Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    DepartemntName = x.Department.Name,
                    x.Salary,
                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            foreach (var emp in query)
            {
                result.AppendLine($"{emp.FirstName} {emp.LastName} from {emp.DepartemntName} - ${emp.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();
            var query = db.Employees
                .Where(x => x.Salary > 50000)
                .Select(x => new
                {
                    x.FirstName,
                    x.Salary,
                })
            .OrderBy(x => x.FirstName)
            .ToList();

            query.ToList().ForEach(x => result.AppendLine($"{x.FirstName} - {x.Salary:f2}".ToString()));
            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext db)
        {
            StringBuilder result = new StringBuilder();

            var query = db.Employees.Select(x => new
            {
                x.EmployeeId,
                x.FirstName,
                x.MiddleName,
                x.LastName,
                x.JobTitle,
                x.Salary,
            }).ToList()
            .OrderBy(x => x.EmployeeId);


            foreach (var employee in query)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return result.ToString().Trim();
        }
    }
}
