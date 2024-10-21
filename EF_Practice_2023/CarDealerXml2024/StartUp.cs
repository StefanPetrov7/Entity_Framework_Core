using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main()
        {
            var databaseContext = new CarDealerContext();

            // => Problem 9

            //string inputSuppliers = File.ReadAllText("../../../Datasets/suppliers.xml");
            //string resultImportSuppliers = ImportSuppliers(databaseContext, inputSuppliers);
            //Console.WriteLine(resultImportSuppliers);

            // => Problem 10

            //string inputParts = File.ReadAllText("../../../Datasets/parts.xml");
            //string resultInputParts = ImportParts(databaseContext, inputParts);
            //Console.WriteLine(resultInputParts);

            // => Problem 11

            //string inputCars = File.ReadAllText("../../../Datasets/cars.xml");
            //string resultInputCars = ImportCars(databaseContext, inputCars);
            //Console.WriteLine(resultInputCars);

            // => Problem 12

            //string inputCustomers = File.ReadAllText("../../../Datasets/customers.xml");
            //string resultInputCustomers = ImportCustomers(databaseContext, inputCustomers);
            //Console.WriteLine(resultInputCustomers);


            // => Problem 13

            //string inputSales = File.ReadAllText("../../../Datasets/sales.xml");
            //string resultInputSales = ImportSales(databaseContext, inputSales);
            //Console.WriteLine(resultInputSales);

            // => Problem 14

            //string resultCarExport = GetCarsWithDistance(databaseContext);
            //Console.WriteLine(resultCarExport);

            // => Problem 15

            //string resultBmwExport = GetCarsFromMakeBmw(databaseContext);
            //Console.WriteLine(resultBmwExport);

            // => Problem 16

            //string resultLocalSuppliersExport = GetLocalSuppliers(databaseContext);
            //Console.WriteLine(resultLocalSuppliersExport);

            // => Problem 17

            //string resultCarsPartsExport = GetCarsWithTheirListOfParts(databaseContext);
            //Console.WriteLine(resultCarsPartsExport);

            // => Problem 18

            //string resultSalesByCustomer = GetTotalSalesByCustomer(databaseContext);
            //Console.WriteLine(resultSalesByCustomer);

            // => Problem 19

            string resultSalesWithoutDiscount = GetSalesWithAppliedDiscount(databaseContext);
            Console.WriteLine(resultSalesWithoutDiscount);


        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            // Manual mapped 
            // Not tested in JUDGE file 17KB

            var query = context.Sales
                .Select(x => new ExportSaleModelXml
                {
                    Car = new ExportCarSaleModelXml
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TraveledDistance = x.Car.TraveledDistance,
                    },
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartsCars.Sum(x => x.Part.Price),
                    PriceWithDiscount = x.Car.PartsCars.Sum(x => x.Part.Price) - x.Discount,
                })
                .ToArray();

            XmlSerializer xmlResult = new XmlSerializer(typeof(ExportSaleModelXml[]), new XmlRootAttribute("sales"));
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty);
            var writer = new StringWriter();
            xmlResult.Serialize(writer, query, nameSpace);

            return writer.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            // Obj were manual mapped

            var query = context.Customers.Where(x => x.Sales.Count > 0)
                .Select(x => new ExportCustomerSalesModelXml
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpendMoney = x.IsYoungDriver ? x.Sales.Select(s => s.Car).SelectMany(x => x.PartsCars).Sum(x => x.Part.Price) * 0.95M
                    : x.Sales.Select(s => s.Car).SelectMany(x => x.PartsCars).Sum(x => x.Part.Price),
                })
                .OrderBy(x => x.SpendMoney)
                .ToArray();

            XmlSerializer xmlResult = new XmlSerializer(typeof(ExportCustomerSalesModelXml[]), new XmlRootAttribute("customers"));
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty);
            using StringWriter writer = new StringWriter();
            xmlResult.Serialize(writer, query, nameSpace);

            return writer.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            // 17KB to bug fo JUDGE => Solution is working 

            // Note that the sort for the part array which is nested into the DTO is done in the mapper configuration.
            // Different option will be to proceed wit foreach and to sort each car => parts before serializing 
            // Example below:
            // foreach (var car in query)
            // {
            //    car.Parts = car.Parts.OrderByDescending(x => x.Name).ToList();
            // }

            InitializeMapper();

            var query = context.Cars
                .OrderByDescending(x => x.TraveledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ProjectTo<ExportCarsPartsModelXml>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer xmlResult = new XmlSerializer(typeof(ExportCarsPartsModelXml[]), new XmlRootAttribute("cars"));
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty);
            using var writer = new StringWriter();
            xmlResult.Serialize(writer, query, nameSpace);

            return writer.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            InitializeMapper();

            var query = context.Suppliers.Where(x => x.IsImporter == false)
                .ProjectTo<ExportSupplierModelXml>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer resultSerializer = new XmlSerializer(typeof(ExportSupplierModelXml[]), new XmlRootAttribute("suppliers"));
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty);
            using var writer = new StringWriter();
            resultSerializer.Serialize(writer, query, nameSpace);

            return writer.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            InitializeMapper();

            var query = context.Cars.Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TraveledDistance)
                .ProjectTo<ExportBmwModelXml>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer resultXml = new XmlSerializer(typeof(ExportBmwModelXml[]), new XmlRootAttribute("cars"));
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty);
            using var writer = new StringWriter();
            resultXml.Serialize(writer, query, nameSpace);

            return writer.ToString().TrimEnd();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            InitializeMapper();

            var query = context.Cars.Where(x => x.TraveledDistance > 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ProjectTo<ExportCarModelXml>(mapper.ConfigurationProvider)
                .ToArray();

            XmlSerializer xmlResult = new XmlSerializer(typeof(ExportCarModelXml[]), new XmlRootAttribute("cars"));
            var nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add("", "");
            using var writer = new StringWriter();
            xmlResult.Serialize(writer, query, nameSpace);

            return writer.ToString().TrimEnd();
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            InitializeMapper();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportSaleModelXml[]), new XmlRootAttribute("Sales"));
            var salesDto = serializer.Deserialize(new StringReader(inputXml)) as ImportSaleModelXml[];
            var carIds = context.Cars.Select(x => x.Id);
            salesDto = salesDto.Where(x => carIds.Contains(x.CarId)).ToArray();
            var sales = mapper.Map<IEnumerable<Sale>>(salesDto);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}"; ;
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            InitializeMapper();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCustomerModelXml[]), new XmlRootAttribute("Customers"));
            using StringReader input = new StringReader(inputXml);
            var customersDto = serializer.Deserialize(input) as IEnumerable<ImportCustomerModelXml>;
            var customers = mapper.Map<IEnumerable<Customer>>(customersDto);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}"; ;
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            InitializeMapper();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCarModelXml[]), new XmlRootAttribute("Cars"));
            using StringReader input = new StringReader(inputXml);
            var carsDtos = serializer.Deserialize(input) as ImportCarModelXml[];
            var validPartIds = context.Parts.Select(part => part.Id).ToHashSet();

            // => Manual mapping 

            //var cars = new List<Car>();

            //foreach (var car in carsDtos)
            //{
            //    Car curCar = new Car
            //    {
            //        Make = car.Make,
            //        Model = car.Model,
            //        TraveledDistance = car.TraveledDistance,
            //    };

            //    foreach (var partId in car.Parts.Select(x => x.Id).Distinct())
            //    {
            //        if (validPartIds.Contains(partId))
            //        {
            //            var partCar = new PartCar { PartId = partId };
            //            curCar.PartsCars.Add(partCar);
            //        }
            //    }

            //    cars.Add(curCar);
            //}

            // => Using mapper

            foreach (var car in carsDtos)
            {
                car.Parts = car.Parts.Where(x => validPartIds.Contains(x.Id)).ToList();
                car.Parts = car.Parts
                .GroupBy(part => part.Id)
                .Select(group => group.First())
                .ToList();
            }

            var cars = mapper.Map<IEnumerable<Car>>(carsDtos);

            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count()}"; ;
        }


        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            InitializeMapper();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportPartModelXml[]), new XmlRootAttribute("Parts"));
            using StringReader input = new StringReader(inputXml);
            var partsDto = serializer.Deserialize(input) as ImportPartModelXml[];

            var supplierIds = context.Suppliers.Select(x => x.Id).ToHashSet();

            var validParts = new List<ImportPartModelXml>();

            foreach (var part in partsDto)
            {
                if (supplierIds.Contains(part.SupplierId))
                {
                    validParts.Add(part);
                }
            }

            var parts = mapper.Map<IEnumerable<Part>>(validParts);
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            InitializeMapper();

            // => Using XML Helper Class! <optional => good practice>
            //var xmlHelper = new XmlHelper();   
            //var supplierDto = xmlHelper.Deserialize<ImportSupplierModelXml[]>(inputXml, "Suppliers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierModelXml[]), new XmlRootAttribute("Suppliers"));
            using var textRead = new StringReader(inputXml);
            var supplierDto = xmlSerializer.Deserialize(textRead) as ImportSupplierModelXml[];

            var suppliers = mapper.Map<IEnumerable<Supplier>>(supplierDto);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}"; ;
        }

        public static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}