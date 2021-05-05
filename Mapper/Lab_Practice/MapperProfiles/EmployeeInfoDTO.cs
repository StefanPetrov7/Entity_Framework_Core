using AutoMapper;
using Lab_Practice.Models;

namespace Mapper_Lab.MapperProfiles
{
    public class EmployeeInfoDTO : Profile
    {
        public EmployeeInfoDTO()
        {
            this.CreateMap<Employees, EmployeeDto>();  // Employees => source
                                                       // Employees = target
        }
    }

    public class EmployeeDto   // DTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string Town { get; set; }
    }
}
