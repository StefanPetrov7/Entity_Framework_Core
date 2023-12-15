namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Serializer
    {
        static IMapper mapper;

        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            InitializeMapper();

            var query = context.Prisoners.ToList().Where(x => ids.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name,
                    })
                   .OrderBy(x => x.OfficerName)
                   .ToList(),
                    TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers.Sum(x => x.Officer.Salary).ToString("F2"))
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToList();

            // Need to work on mapping the officers collection!!

            //var query = context.Prisoners.Where(x => ids.Contains(x.Id))
            //.ProjectTo<ExPrisonerDto>(mapper.ConfigurationProvider)
            //.ToList();

            var jsonQuery = JsonConvert.SerializeObject(query, Formatting.Indented);
            return jsonQuery.ToString();
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var names = prisonersNames.Split(",").ToArray();

            var query = context.Prisoners.Where(x => prisonersNames.Contains(x.FullName))
                .Select(x => new ExPrisonerXmlDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    Messages = x.Mails.Select(x => new MessagesXmlDto
                    {
                        Description = String.Join("", x.Description.Reverse()),
                    }).ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExPrisonerXmlDto[]), new XmlRootAttribute("Prisoners"));
            var nameSapces = new XmlSerializerNamespaces();
            nameSapces.Add("", "");
            var xmlResultWriter = new StringWriter();
            xmlSerializer.Serialize(xmlResultWriter, query, nameSapces);

            return xmlResultWriter.ToString();
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


