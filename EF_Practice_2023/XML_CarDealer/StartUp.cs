using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Castle.Core.Resource;
using CarDealer.DTOs.Export;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main()
        {
            var db = new CarDealerContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //string importSuppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(db, importSuppliersXml));

            //string importPartsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(db, importPartsXml));

            //string importCars = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(db, importCars));

            //string importCustomers = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(db, importCustomers));

            //string importSales = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(db, importSales));

            //Console.WriteLine(GetCarsWithDistance(db));

            //Console.WriteLine(GetCarsFromMakeBmw(db));

            //Console.WriteLine(GetLocalSuppliers(db));

            //Console.WriteLine(GetCarsWithTheirListOfParts(db));

            // Console.WriteLine(GetTotalSalesByCustomer(db));

            Console.WriteLine(GetSalesWithAppliedDiscount(db));

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesQuery = context.Sales.Select(x => new SaleExportModel
            {
                Car = new CarExportModelAttributes
                {
                    Make = x.Car.Make,
                    Model = x.Car.Model,
                    TraveledDistance = x.Car.TraveledDistance,
                },
                Discount = x.Discount,
                CustomerName = x.Customer.Name,
                Price = x.Car.PartsCars.Sum(x => x.Part.Price),
                PriceWithDiscount = x.Car.PartsCars.Sum(x => x.Part.Price) - (x.Car.PartsCars.Sum(x => x.Part.Price) * x.Discount / 100)
            }).ToArray();

            var xmlSalesDiscount = new XmlSerializer(typeof(SaleExportModel[]), new XmlRootAttribute("sales"));

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            var xmlWriter = new StringWriter();
            xmlSalesDiscount.Serialize(xmlWriter, salesQuery, nameSpaces);
            return xmlWriter.ToString();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            // Not giving points !!!

            var customerQuery = context.Customers.Where(c => c.Sales.Any())
                .Select(x => new CustomersExportModel
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Select(x => x.Car).SelectMany(x => x.PartsCars).Sum(x => x.Part.Price) - x.Sales.Sum(x => x.Discount)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            XmlSerializer xmlCustomers = new XmlSerializer(typeof(CustomersExportModel[]), new XmlRootAttribute("customers"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var xmlWriter = new StringWriter();
            xmlCustomers.Serialize(xmlWriter, customerQuery, namespaces);

            return xmlWriter.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsQuery = context.Cars.Select(c => new CarsExportModel
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance,
                Parts = c.PartsCars.Select(p => new PartExportModel
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price,
                })
                .OrderByDescending(x => x.Price)
                .ToArray(),
            })
                .OrderByDescending(x => x.TraveledDistance)
                .ThenBy(x => x.Model).Take(5)
                .ToArray();

            XmlSerializer carsSerializer = new XmlSerializer(typeof(CarsExportModel[]), new XmlRootAttribute("cars"));

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            var writer = new StringWriter();

            carsSerializer.Serialize(writer, carsQuery, nameSpaces);
            return writer.ToString();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var supplierQuer = context.Suppliers.Where(x => x.IsImporter == false)
                .Select(x => new SupplierExportModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                }).ToArray();

            XmlSerializer xmlQuery = new XmlSerializer(typeof(SupplierExportModel[]), new XmlRootAttribute("suppliers"));

            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add("", "");

            var writer = new StringWriter();
            xmlQuery.Serialize(writer, supplierQuer, nameSpace);
            return writer.ToString();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwWuery = context.Cars.Where(x => x.Make == "BMW")
                .Select(x => new BmwExportModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TraveledDistance)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BmwExportModel[]), new XmlRootAttribute("cars"));

            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add("", "");

            var xmlWriter = new StringWriter();
            xmlSerializer.Serialize(xmlWriter, bmwWuery, nameSpace);

            return xmlWriter.ToString();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carQuery = context.Cars.Where(x => x.TraveledDistance > 2_000_000)
                .Select(x => new CarExportModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToArray();

            XmlSerializer xmlResult = new XmlSerializer(typeof(CarExportModel[]), new XmlRootAttribute("Cars"));

            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add("", "");  // to update the version tag  => <?xml version="1.0" encoding="utf-16"?>

            var writer = new StringWriter();
            xmlResult.Serialize(writer, carQuery, nameSpace);

            return writer.ToString();
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            CreateMapperInstance();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaleImportModel[]), new XmlRootAttribute("Sales"));
            var salesDtos = xmlSerializer.Deserialize(new StringReader(inputXml)) as SaleImportModel[];
            int[] carsValidIds = context.Cars.Select(x => x.Id).ToArray();
            int[] customerValidIds = context.Customers.Select(x => x.Id).ToArray();

            salesDtos = salesDtos.Where(x => carsValidIds.Contains(x.CarId)).ToArray();
            // salesDtos = salesDtos.Where(x => customerValidIds.Contains(x.CustomerId)).ToArray();  // => Not needed for the Task!!!!

            var sales = mapper.Map<Sale[]>(salesDtos);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            CreateMapperInstance();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomersImportModel[]), new XmlRootAttribute("Customers"));
            var customersDtos = xmlSerializer.Deserialize(new StringReader(inputXml)) as CustomersImportModel[];
            Customer[] customers = mapper.Map<Customer[]>(customersDtos);
            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            CreateMapperInstance();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarsImportModel[]), new XmlRootAttribute("Cars"));

            var carsDtos = xmlSerializer.Deserialize(new StringReader(inputXml)) as CarsImportModel[];

            int[] partsIds = context.Parts.Select(x => x.Id).ToArray();
            List<Car> cars = new List<Car>();

            foreach (var curCar in carsDtos)
            {
                Car car = new Car()
                {
                    Make = curCar.Make,
                    Model = curCar.Model,
                    TraveledDistance = curCar.TraveledDistance,
                };

                foreach (var partId in curCar.Parts.Select(x => x.Id).Distinct())
                {
                    if (partsIds.Contains(partId))
                    {
                        car.PartsCars.Add(new PartCar { PartId = partId });
                    }
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            CreateMapperInstance();

            var suppliersIds = context.Suppliers.Select(x => x.Id).ToList();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PartsImportModel[]), new XmlRootAttribute("Parts"));
            var partsDTOs = xmlSerializer.Deserialize(new StringReader(inputXml)) as PartsImportModel[];
            partsDTOs = partsDTOs.Where(p => suppliersIds.Contains(p.SupplierId)).ToArray();
            var parts = mapper.Map<Part[]>(partsDTOs);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            CreateMapperInstance();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SuppliersImportModel[]), new XmlRootAttribute("Suppliers"));   // creating serializer from => the xml providing the root name and the dto class
            var textRead = new StringReader(inputXml);     // imputing the xml string with the string reader since the desirializer is accepting that format
            var suppliersDto = xmlSerializer.Deserialize(textRead) as SuppliersImportModel[];    // desirializing the xml into the dto obj 

            //var suppliers = suppliersDto.Select(x => new                     // no mapper, manual parsing from suppliersDto to Supplier 
            //{
            //    name = x.Name,
            //    isImporter = x.IsImporter,
            //}).ToArray();

            var suppliers = mapper.Map<IEnumerable<Supplier>>(suppliersDto);   // using mapper 
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}"; ;
        }

        public static void CreateMapperInstance()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();

        }
    }
}