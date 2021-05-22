using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var db = new CarDealerContext();

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //string jsSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");
            //string jsParts = File.ReadAllText("../../../Datasets/parts.json");
            //string jsCars = File.ReadAllText("../../../Datasets/cars.json");
            //string jsCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //string jsSales = File.ReadAllText("../../../Datasets/sales.json");

            //Console.WriteLine(ImportSuppliers(db, jsSuppliers));
            //Console.WriteLine(ImportParts(db, jsParts));
            //Console.WriteLine(ImportCars(db, jsCars));
            //Console.WriteLine(ImportCustomers(db, jsCustomers));
            //Console.WriteLine(ImportSales(db, jsSales));

            //Console.WriteLine(GetOrderedCustomers(db));
            //Console.WriteLine(GetCarsFromMakeToyota(db));
            //Console.WriteLine(GetLocalSuppliers(db));
            //Console.WriteLine(GetCarsWithTheirListOfParts(db));
            //Console.WriteLine(GetTotalSalesByCustomer(db));
            //Console.WriteLine(GetSalesWithAppliedDiscount(db));
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext db)
        {
            var sales = db.Sales.Select(c => new
            {
                car = new
                {
                    c.Car.Make,
                    c.Car.Model,
                    c.Car.TravelledDistance,
                },
                customerName = c.Customer.Name,
                Discount = c.Discount.ToString("F2"),
                price = c.Car.PartCars.Sum(x => x.Part.Price).ToString("F2"),
                priceWithDiscount = (c.Car.PartCars.Sum(x => x.Part.Price) * (100 - c.Discount) / 100).ToString("F2"),
            })
            .Take(10)
            .ToList();

            var jsonResult = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return jsonResult;

        }

        public static string GetTotalSalesByCustomer(CarDealerContext db)
        {
            var customers = db.Customers
                .Where(x => x.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(m => m.Car.PartCars.Sum(s => s.Part.Price)),
                })
                .ToList()
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars);

            var jsonRresult = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return jsonRresult;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext db)
        {
            var cars = db.Cars.Select(c => new
            {
                car = new
                {
                    c.Make,
                    c.Model,
                    c.TravelledDistance,
                },
                parts = c.PartCars.Select(p => new

                {
                    Name = p.Part.Name,
                    Price = p.Part.Price.ToString("f2")

                }).ToList()

            }).ToList();

            var jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonResult;
        }

        public static string GetLocalSuppliers(CarDealerContext db)
        {
            var supliers = db.Suppliers.Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count,

                })
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(supliers, Formatting.Indented);

            return jsonResult;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext db)
        {
            var cars = db.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance,
                })
                .ToList()
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance);

            var jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonResult;

        }

        public static string GetOrderedCustomers(CarDealerContext db)
        {

            var customers = db.Customers.OrderBy(x => x.BirthDate).ThenBy(x => x.IsYoungDriver).Select(x => new
            {
                x.Name,
                BirthDate = x.BirthDate.ToString("dd/M/yyyy"),
                x.IsYoungDriver,
            })
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return jsonResult;

        }

        public static string ImportSales(CarDealerContext db, string inputJson)
        {
            InitializeMapper();

            var salesDto = JsonConvert.DeserializeObject<IEnumerable<ImportSalesModel>>(inputJson);

            var sales = mapper.Map<IEnumerable<Sale>>(salesDto);

            db.Sales.AddRange(sales);

            db.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext db, string inputJson)
        {

            InitializeMapper();

            var customersDto = JsonConvert.DeserializeObject<IEnumerable<ImportCustomersModel>>(inputJson);

            var customers = mapper.Map<IEnumerable<Customer>>(customersDto);

            db.Customers.AddRange(customers);

            db.SaveChanges();

            return $"Successfully imported {customers.Count()}.";

        }

        public static string ImportCars(CarDealerContext db, string inputJson)  
        {
            var carsDto = JsonConvert.DeserializeObject<IEnumerable<ImportCarModel>>(inputJson);

            var cars = new List<Car>();

            foreach (var car in carsDto)
            {
                var curCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                };

                foreach (var partId in car?.PartsId.Distinct())
                {
                    curCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                cars.Add(curCar);
            }

            db.Cars.AddRange(cars);

            db.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        public static string ImportParts(CarDealerContext db, string inputJson)
        {
            InitializeMapper();

            var supplierId = db.Suppliers.Select(x => x.Id).ToArray();

            var partsDto = JsonConvert.DeserializeObject<IEnumerable<ImportPartModel>>(inputJson)
                .Where(x => supplierId.Contains(x.SupplierId))
                .ToList();

            var parts = mapper.Map<IEnumerable<Part>>(partsDto);

            db.Parts.AddRange(parts);

            db.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext db, string inputJson)
        {
            InitializeMapper();

            var suppliersDto = JsonConvert.DeserializeObject<IEnumerable<ImportSuppliersModel>>(inputJson);

            var suppliers = mapper.Map<IEnumerable<Supplier>>(suppliersDto);

            db.Suppliers.AddRange(suppliers);

            db.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        public static void InitializeMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = mapperCfg.CreateMapper();
        }
    }
}