using System;
using System.Linq;
using System.Text;
using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

            var dbContext = new SoftUniContext();

            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(dbContext));

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
