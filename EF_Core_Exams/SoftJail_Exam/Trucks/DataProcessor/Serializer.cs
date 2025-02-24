namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var query = context.Despatchers.Where(x => x.Trucks.Count >= 1)
                .Select(x => new ExportDespathcerModelXml
                {
                    TrucksCount = x.Trucks.Count,
                    DespatcherName = x.Name,
                    Trucks = x.Trucks.Select(x => new ExportTruckModelXml
                    {
                        RegistrationNumber = x.RegistrationNumber,
                        Make = x.MakeType.ToString(),
                    })
                    .OrderBy(x => x.RegistrationNumber)
                    .ToList()
                })
                .OrderByDescending(x => x.TrucksCount)
                .ThenBy(x => x.DespatcherName)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportDespathcerModelXml[]), new XmlRootAttribute("Despatchers"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var xmlResult = new StringWriter();
            xmlSerializer.Serialize(xmlResult, query, namespaces);
            return xmlResult.ToString().TrimEnd();    

        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var query = context.Clients.Where(x => x.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .Select(x => new
                {
                    x.Name,
                    Trucks = x.ClientsTrucks
                    .Where(x => x.Truck.TankCapacity >= capacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        t.Truck.VinNumber,
                        t.Truck.TankCapacity,
                        t.Truck.CargoCapacity,
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = ((MakeType)t.Truck.MakeType).ToString(),   // Additional calling to Array as EF core cannot perform sorting in SQL from enum. 
                    })
                    .ToArray()
                })
                .ToArray()
                .OrderByDescending(x => x.Trucks.Length)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    Trucks = x.Trucks.OrderBy(x => x.MakeType)
                    .ThenByDescending(x => x.CargoCapacity)
                    .ToArray()
                })
                  .Take(10)
                  .ToArray();

            var jsonResult = JsonConvert.SerializeObject(query, Formatting.Indented);
            return jsonResult.ToString().TrimEnd();



        }
    }
}
