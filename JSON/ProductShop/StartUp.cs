using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;      // initializing mapper field for multiple use purposes 

        public static void Main(string[] args)
        {

            var db = new ProductShopContext();         // Initializing database;
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //string jsonUsers = File.ReadAllText("../../../Datasets/users.json");    // Reading the information from the json file
            //var importUsers = ImportUsers(db, jsonUsers);

            //string productJson = File.ReadAllText("../../../Datasets/products.json");
            //var importProducts = ImportProducts(db, productJson);

            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //var importCategories = ImportCategories(db, categoriesJson);

            //string categoryProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //var categoryProducts = ImportCategoryProducts(db, categoryProductsJson);


            //Console.WriteLine(importUsers);
            //Console.WriteLine(importProducts);
            //Console.WriteLine(importCategories);
            //Console.WriteLine(categoryProducts);

            //Console.WriteLine(GetProductsInRange(db));

            //Console.WriteLine(GetSoldProducts(db));

            //Console.WriteLine(GetCategoriesByProductsCount(db));

            Console.WriteLine(GetUsersWithProducts(db));

        }

        public static string GetUsersWithProducts(ProductShopContext db)
        {
            var users = db.Users
                .Include(x => x.ProductsSold)
                .ToList()
                .Where(u => u.ProductsSold.Any(b => b.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(b => b.BuyerId != null).Count(),
                        products = u.ProductsSold.Where(b => b.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                        })
                    }
                })
                .OrderByDescending(x => x.soldProducts.products.Count());

            var usersCount = new
            {
                usersCount = users.Count(),
                users = users
            };

            var jsOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var resultJson = JsonConvert.SerializeObject(usersCount, Formatting.Indented, jsOptions);

            return resultJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext db)
        {
            var categories = db.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Count == 0 ?
                    0.ToString("F2") : c.CategoryProducts.Average(x => x.Product.Price).ToString("F2"),
                    totalRevenue = c.CategoryProducts.Sum(x => x.Product.Price).ToString("F2"),
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            InitializeAutoMapper();

            //var categories = db.Categories.ProjectTo<ExportProductsByCategoryNameModel>(mapper.ConfigurationProvider)   // => ?Judge score 0
            //    .OrderByDescending(x => x.ProductsCount)
            //    .ToList();

            var jsonResult = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return jsonResult;

        }

        public static string GetSoldProducts(ProductShopContext db)
        {
            var users = db.Users
                .Where(u => u.ProductsSold.Any(b => b.BuyerId != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Where(p => p.BuyerId != null)
                  .Select(y => new
                  {
                      name = y.Name,
                      price = y.Price,
                      buyerFirstName = y.Buyer.FirstName,
                      buyerLastName = y.Buyer.LastName
                  }).ToList()
                })
            .OrderBy(x => x.lastName)
            .ThenBy(x => x.firstName)
            .ToList();


            var jsonSoldProductsResult = JsonConvert.SerializeObject(users, Formatting.Indented);

            // File.WriteAllText("../../../Datasets/soldProductsquery.json", jsonSoldProductsResult);

            return jsonSoldProductsResult;

        }

        public static string GetProductsInRange(ProductShopContext db)
        {
            InitializeAutoMapper();

            var products = db.Products.ProjectTo<ExportProductInRangeModel>(mapper.ConfigurationProvider)
            .Where(x => x.Price >= 500 && x.Price <= 1000)
            .OrderBy(x => x.Price)
            .ToList();

            var jsonResult = JsonConvert.SerializeObject(products, Formatting.Indented);

            //File.WriteAllText("../../../Datasets/requestedProducts.json", jsonResult);   // Not needed for the solution in judge. 

            return jsonResult;
        }

        public static string ImportCategoryProducts(ProductShopContext db, string categoryProductsJson)
        {
            InitializeAutoMapper();

            var categoryProductsDto = JsonConvert.DeserializeObject<IEnumerable<CategoryProductsInputModel>>(categoryProductsJson);

            var categoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoryProductsDto);

            db.CategoryProducts.AddRange(categoryProducts);

            db.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext db, string categoryJson)
        {
            InitializeAutoMapper();

            //var jsonOption = new JsonSerializerSettings()      // JsonConverter option to ignore null values when extracting the obj's.
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //};

            var dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputModel>>(categoryJson)
                .Where(x => x.Name != null)
                .ToList();

            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            db.Categories.AddRange(categories);

            db.SaveChanges();

            return $"Successfully imported {categories.Count()}";

        }

        public static string ImportProducts(ProductShopContext db, string productJson)
        {
            InitializeAutoMapper();

            var dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(productJson);

            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            db.Products.AddRange(products);

            db.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);    // importing obj from the json file, as new DTO obj for import into the db

            var users = mapper.Map<IEnumerable<User>>(dtoUsers);   // maping the DTO UserInputModel as User 

            context.Users.AddRange(users);   // importing the users into the db

            context.SaveChanges();

            return $"Successfully imported {users.Count()}";

        }

        private static void InitializeAutoMapper()
        {
            var mapConfig = new MapperConfiguration(cfg =>      // the first step of creating mapper. 
            {
                cfg.AddProfile<ProductShopProfile>();           // adding the mapping obj configurations from the Profile inherited class
            });

            mapper = mapConfig.CreateMapper();                 // creating mapper
        }
    }
}