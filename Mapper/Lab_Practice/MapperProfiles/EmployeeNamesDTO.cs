using AutoMapper;
using Lab_Practice.Models;

namespace Mapper_Lab.MapperProfiles
{
    public class EmployeeNamesDTO : Profile
    {
        public EmployeeNamesDTO()
        {                // sorce     // target
            this.CreateMap<Employees, EmployeeNamesDto>()
                .ForMember(x => x.FullName, options =>      // Custom prop setting, in case the prop name is not recognized. 
                {
                    options.MapFrom(x => x.FirstName + " " + x.LastName);
                });
        }

    }

    public class EmployeeNamesDto  // DTO 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }
    }
}
