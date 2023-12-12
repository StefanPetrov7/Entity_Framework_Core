using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using Services;
using Services.Contracts;
using Services.Models;
using System.Text;
using System.Xml.Serialization;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var db = new AppDBContext();
            db.Database.Migrate();

            while (true)
            {

                Console.Clear();
                Console.WriteLine("CHOOSE AN OPTION");
                Console.WriteLine("1 => PROPERTY SEARCH");
                Console.WriteLine("2 => MOST EXPENSIVE DISTRICTS");
                Console.WriteLine("3 => AVERAGE PRICE PER M/2");
                Console.WriteLine("4 => ADDING TAGS");
                Console.WriteLine("5 => BULK TAG PROPERTIES");
                Console.WriteLine("6 => PROVIDE PROPERTY INFO");
                Console.WriteLine("0 => EXIT APPLICATION");

                bool parsed = int.TryParse(Console.ReadLine(), out var value);

                if (parsed && value == 0)
                {
                    Environment.Exit(0);
                }

                if (parsed && value >= 1 && value <= 6)
                {
                    switch (value)
                    {
                        case 1:
                            PropertySeacrh(db);
                            break;
                        case 2:
                            MostExpensiveDistrict(db);
                            break;
                        case 3:
                            AveragePricePerM2(db);
                            break;
                        case 4:
                            AddTag(db);
                            break;
                        case 5:
                            BulkTagToProperties(db);
                            break;
                        case 6:
                            GetPropertInfo(db);
                            break;
                    }
                }

                Console.WriteLine("PRESS ANU KEY TO CONTINUE");
                Console.ReadKey();

            }
        }

        private static void GetPropertInfo(AppDBContext db)
        {
            Console.WriteLine("SELECT PROPERTY COUNT");
            int count = int.Parse(Console.ReadLine());

            IPropertyService propertyService = new PropertyService(db);
            var properties = propertyService.GetPropertyFullDataModels(count).ToArray();

            var xmlSerializer = new XmlSerializer(typeof(PropertyFullDataModel[]), new XmlRootAttribute("Properties"));
            var xmlWriter = new StringWriter();
            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");
            xmlSerializer.Serialize(xmlWriter, properties, nameSpaces);

            Console.WriteLine(xmlWriter.ToString().TrimEnd());

            // Regular font output

            //foreach (var prop in properties)
            //{
            //    Console.WriteLine($"{prop.DistrictName}, {prop.BuildingType}, {prop.PropertyType}, M2 {prop.Size},  $ {prop.Price}, {prop.Year}");

            //    foreach (var tag in prop.Tags)
            //    {
            //        Console.WriteLine($"{tag.Name}");
            //    }
            //}
        }

        private static void BulkTagToProperties(AppDBContext db)
        {
            Console.WriteLine("OPRATIONS STARTED!");
            IPropertyService propService = new PropertyService(db);
            ITagService tagService = new TagService(db, propService);
            tagService.BulkTagProperties();
            Console.WriteLine("OPERATION HAS BEEN COMPLETED");
        }

        private static void AddTag(AppDBContext db)
        {
            IPropertyService propService = new PropertyService(db);
            ITagService tagService = new TagService(db, propService);

            Console.WriteLine("INPUT TAG NAME:");
            string tagName = Console.ReadLine();

            Console.WriteLine("OPTIONAL TAG IMPORTANCE LEVEL:");
            bool parsed = int.TryParse(Console.ReadLine(), out int value);
            int? importanceLevel = parsed ? value : null;

            tagService.AddTag(tagName, importanceLevel);

        }

        private static void AveragePricePerM2(AppDBContext db)
        {
            IPropertyService propService = new PropertyService(db);
            var result = propService.AveragePricePerM2();
            Console.WriteLine($"TOTAL AVERAGE PRICE PER M2 {result:0.00}");

        }

        private static void MostExpensiveDistrict(AppDBContext db)
        {
            Console.WriteLine("SELECT DISTRICTS COUNT TO BE DISPLAYED:");
            int count = int.Parse(Console.ReadLine());

            IDistrictService distService = new DistrictService(db);

            var districtsModels = distService.GetMostExpesiveDistricts(count);

            foreach (var dist in districtsModels)
            {
                Console.WriteLine($"{dist.Name}, (AVAILABLE PROPERTIES: {dist.PropertiesCount}), PRICE M2: {dist.AvgPricePerM2: 0.00} ");
            }
        }

        private static void PropertySeacrh(AppDBContext db)
        {
            Console.WriteLine("MIN PRICE:");
            int minPrice = int.Parse(Console.ReadLine());

            Console.WriteLine("MAX PRICE:");
            int maxPrice = int.Parse(Console.ReadLine());

            Console.WriteLine("MIN SIZE:");
            int minSize = int.Parse(Console.ReadLine());

            Console.WriteLine("MAX SIZE:");
            int maxSize = int.Parse(Console.ReadLine());

            IPropertyService propService = new PropertyService(db);
            var propertyModels = propService.Search(minPrice, maxPrice, minSize, maxSize);

            // TODO => XML Output

            foreach (var prop in propertyModels)
            {
                Console.WriteLine($"{prop.DistrictName}, {prop.BuildingType}, {prop.PropertyType}, {prop.Size} M2, {prop.Price} EURO");
            }
        }
    }
}
