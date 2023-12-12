
using Data;
using Importer.Models;
using Services;
using Services.Contracts;
using System.Text.Json;

namespace Importer
{
    public class Program
    {
        static void Main(string[] args)
        {

            ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
            ImportJsonFile("imot.bg-raw-data-2021-03-18.json");

        }

        public static void ImportJsonFile(string fileLocation)
        {
            var db = new AppDBContext();

            IPropertyService propServices = new PropertyService(db);

            var jsonProperties = JsonSerializer.Deserialize<IEnumerable<PropertyJsonModel>>(File.ReadAllText(fileLocation));

            foreach (var prop in jsonProperties)
            {
                propServices.AddProperty(prop.District, prop.Floor, prop.TotalFloor, prop.Size, prop.YardSize, prop.Year, prop.Type, prop.BuildingType, prop.Price);
                Console.Write(".");
            }
        }
    }
}
