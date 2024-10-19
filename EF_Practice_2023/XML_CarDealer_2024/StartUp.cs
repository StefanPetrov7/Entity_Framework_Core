using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
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

        }

        public static string ImportSales(CarDealerContext context, string inputXml) 
        {
            InitializeMapper();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportSaleModelXml[]), new XmlRootAttribute("Sales"));
            var salesDto = serializer.Deserialize(new StringReader(inputXml)) as ImportSaleModelXml[];
            var carIds = context.Cars.Select(x => x.Id);
            salesDto = salesDto.Where(x=> carIds.Contains(x.CarId)).ToArray();
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
            //context.SaveChanges();

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