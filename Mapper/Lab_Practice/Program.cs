using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;

using Lab_Practice.Models;
using Mapper_Lab.MapperProfiles;

namespace Mapper_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            // First Step of using AutoMapper

            var startConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new EmployeeInfoDTO());       // Using a Profile class to create DTO classes. 
                config.AddProfile(new EmployeeNamesDTO());
            });

            // Second Step of using AutoMapper

            var employee = db.Employees.FirstOrDefault(x => x.FirstName == "Stef");

            var mapper = startConfig.CreateMapper();

            var employeeDto = mapper.Map<EmployeeDto>(employee);

            var empNamesOnlyDto = mapper.Map<EmployeeNamesDto>(employee);

            Console.WriteLine(JsonConvert.SerializeObject(employeeDto, Formatting.Indented));

            Console.WriteLine(JsonConvert.SerializeObject(empNamesOnlyDto, Formatting.Indented));


            // Replacing SELECT and directly extracting the required DTO's from the DB. 

            var employeesInfo = db.Employees.ProjectTo<EmployeeNamesDto>(startConfig).ToList();

            //foreach (var emp in employeeInfo)
            //{
            //    Console.WriteLine($"{emp.FirstName}/{emp.LastName}/{emp.JobTitle}/{emp.Town}");
            //}

            Console.WriteLine(JsonConvert.SerializeObject(employeesInfo, Formatting.Indented));   // Testing Json

        }

        public static IEnumerable<EmployeeDto> GetEmpInfo(SoftUniContext db)  // Replaced with => using AutoMapper.QueryableExtensions;
        {
            var list = db.Employees
               .Select(x => new EmployeeDto
               {
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   JobTitle = x.JobTitle,
                   Town = x.Address.Town.Name,

               })
               .ToList();

            return list;
        }
    }
}
