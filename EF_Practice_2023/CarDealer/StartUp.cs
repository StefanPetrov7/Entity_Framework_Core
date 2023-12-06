using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            //string jsonSupInput = File.ReadAllText("../../../Datasets/suppliers.json");
            //string importSupplierResult = ImportSuppliers(db, jsonSupInput);
            //Console.WriteLine(importSupplierResult);

            //string jsonPartsInput = File.ReadAllText("../../../Datasets/parts.json");
            //string importPartsResult = ImportParts(db, jsonPartsInput);
            //Console.WriteLine(importPartsResult);

            //string importJsonCars = File.ReadAllText("../../../Datasets/cars.json");
            //string carsResult = ImportCars(db, importJsonCars);
            //Console.WriteLine(carsResult);

            //string importJsonCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //string customersResult = ImportCustomers(db, importJsonCustomers);
            //Console.WriteLine(customersResult);

            //string importJsonSales = File.ReadAllText("../../../Datasets/sales.json");
            //string customersResult = ImportSales(db, importJsonSales);
            //Console.WriteLine(customersResult);

            //string exportOrders = GetOrderedCustomers(db);
            //Console.WriteLine(exportOrders);

            //string exportToyotaCars = GetCarsFromMakeToyota(db);
            //Console.WriteLine(exportToyotaCars);

            //string exportSuppliers = GetLocalSuppliers(db);
            //Console.WriteLine(exportSuppliers);

            //string exportCarsWithParts = GetCarsWithTheirListOfParts(db);
            //Console.WriteLine(exportCarsWithParts);

            //string exportSalesByCustomer = GetTotalSalesByCustomer(db);
            //Console.WriteLine(exportSalesByCustomer);

            string exportSalesWithDiscounts = GetSalesWithAppliedDiscount(db);
            Console.WriteLine(exportSalesWithDiscounts);

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var query = context.Sales.Select(x => new
            {
                car = new
                {
                    Make = x.Car.Make,
                    Model = x.Car.Model,
                    TraveledDistance = x.Car.TraveledDistance,
                },

                customerName = x.Customer.Name,
                discount = x.Discount.ToString("F2"),
                price = x.Car.PartsCars.Sum(x => x.Part.Price).ToString("F2"),
                priceWithDiscount = (x.Car.PartsCars.Sum(x => x.Part.Price) * (100 - x.Discount) / 100).ToString("F2")
            })
            .ToList()
            .Take(10);

            return JsonConvert.SerializeObject(query, Formatting.Indented);
        }



        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            // Passing the test with 100 point, givining the error when printing on he console! 

            var query = context.Customers
              .Where(x => x.Sales.Count >= 1)
              .Select(c => new
              {
                  fullName = c.Name,
                  boughtCars = c.Sales.Count,
                  spentMoney = c.Sales.Sum(m => m.Car.PartsCars.Sum(s => s.Part.Price)),
              })
              .ToList()
              .OrderByDescending(x => x.spentMoney)
              .ThenByDescending(x => x.boughtCars);

            return JsonConvert.SerializeObject(query, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var query = context.Cars.Select(c => new
            {
                car = new
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                },
                parts = c.PartsCars.Select(p => new
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price.ToString("F2"),
                })
            }).ToList();

            return JsonConvert.SerializeObject(query, Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var query = context.Suppliers.Where(x => x.IsImporter == false)
            .Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                PartsCount = x.Parts.Count,

            }).ToList();

            return JsonConvert.SerializeObject(query, Formatting.Indented);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var query = context.Cars.Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TraveledDistance)
                .ToList();

            return JsonConvert.SerializeObject(query, Formatting.Indented);
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var query = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver,
                })
                .ToList(); ;

            return JsonConvert.SerializeObject(query, Formatting.Indented);
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            CreateMapperInstance();
            var saleDto = JsonConvert.DeserializeObject<IEnumerable<SalesImportModel>>(inputJson);
            var sales = mapper.Map<IEnumerable<Sale>>(saleDto);
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            CreateMapperInstance();

            var customersDto = JsonConvert.DeserializeObject<IEnumerable<CustomersModelsImport>>(inputJson);
            var customers = mapper.Map<IEnumerable<Customer>>(customersDto);
            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            CreateMapperInstance();

            var carsDto = JsonConvert.DeserializeObject<IEnumerable<CarImportModel>>(inputJson);

            var validPartsIds = context.Parts.Select(x => x.Id).ToList();

            List<Car> cars = new List<Car>();

            foreach (var carDto in carsDto)
            {
                Car currentCar = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance,
                };

                foreach (var currentPartId in carDto.PartsId.Distinct())
                {
                    if (validPartsIds.Contains(currentPartId))
                    {
                        currentCar.PartsCars.Add(new PartCar { PartId = currentPartId });
                    }
                }

                cars.Add(currentCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            CreateMapperInstance();

            var supliersIds = context.Suppliers
                .Select(x => x.Id).ToList();

            var dtoParts = JsonConvert.DeserializeObject<IEnumerable<PartsImportModel>>(inputJson)
                .Where(x => supliersIds
                .Contains(x.SupplierId))
                .ToList();

            var parts = mapper.Map<IEnumerable<Part>>(dtoParts);
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            CreateMapperInstance();

            var dtoSuppliers = JsonConvert.DeserializeObject<IEnumerable<SuplierImportModel>>(inputJson);
            var suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        public static void CreateMapperInstance()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}