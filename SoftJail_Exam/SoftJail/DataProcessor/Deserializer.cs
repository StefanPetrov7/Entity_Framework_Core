namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System.Text.RegularExpressions;
    using System.Runtime.Intrinsics.Arm;
    using System.Xml.Serialization;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        static IMapper mapper;

        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            InitializeMapper();

            var departmentsDto = JsonConvert.DeserializeObject<IEnumerable<DepartmentCells>>(jsonString);

            var filteredDto = new List<DepartmentCells>();

            foreach (var department in departmentsDto)
            {
                if (IsValid(department) == false
                    || department.Cells.Any() == false
                    || department.Cells.All(IsValid) == false)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }
                result.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
                filteredDto.Add(department);
            }

            var departments = mapper.Map<IEnumerable<Department>>(filteredDto);

            context.Departments.AddRange(departments);

            context.SaveChanges();

            return result.ToString().TrimEnd();

        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            InitializeMapper();

            var prisonersDto = JsonConvert.DeserializeObject<IEnumerable<PrisonerDto>>(jsonString);

            var validatedDto = new List<PrisonerDto>();

            foreach (var prisoner in prisonersDto)
            {
                if (IsValid(prisoner) == false || prisoner.Mails.All(IsValid) == false)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                result.AppendLine(string.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));

                validatedDto.Add(prisoner);
            }

            var prisoners = mapper.Map<IEnumerable<Prisoner>>(validatedDto);

            context.Prisoners.AddRange(prisoners);

            context.SaveChanges();

            return result.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder result = new StringBuilder();

            InitializeMapper();

            var xmlInput = new  XmlSerializer(typeof(OfficersXmlDto[]), new XmlRootAttribute("Officers"));
            var xmlReader = new StringReader(xmlString);

            var officeDto = xmlInput.Deserialize(xmlReader) as OfficersXmlDto[];

            var validatedOfficers = new List<OfficersXmlDto>();

            //var officersNoMapper = new List<Officer>();

            foreach (var officer in officeDto)
            {
                if (IsValid(officer) == false)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                result.AppendLine(string.Format(SuccessfullyImportedOfficer, officer.FullName, officer.OfficerPrisoners.Count()));

                //var noMApperOfficer = new Officer
                //{
                //    FullName = officer.FullName,    
                //    Salary = officer.Salary,
                //    DepartmentId = officer.DepartmentId,    
                //    Position = Enum.Parse<Position>(officer.Position),
                //    Weapon = Enum.Parse<Weapon>(officer.Weapon),
                //    OfficerPrisoners = officer.OfficerPrisoners.Select(x=> new OfficerPrisoner 
                //    {
                //        PrisonerId = x.Id,
                //    }).ToList()
                //};

                //officersNoMapper.Add(noMApperOfficer);  

                validatedOfficers.Add(officer); 
            }

            var officers = mapper.Map<IEnumerable<Officer>>(validatedOfficers);

            context.Officers.AddRange(officers);

            context.SaveChanges();
            
            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }

        public static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SoftJailProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}