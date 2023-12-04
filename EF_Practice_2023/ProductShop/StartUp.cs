using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Runtime.CompilerServices;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main()
        {
            var db = new ProductShopContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            // => TASK 1

            //string inputJson = File.ReadAllText("../../../Datasets/users.json");  // importing the JSON file into string C# file
            //var resultsInputUsers = ImportUsers(db, inputJson);
            //Console.WriteLine(resultsInputUsers);

            // => TASK 2

            //string inputJsonProducts = File.ReadAllText("../../../Datasets/products.json");
            //string resultInputProducts = ImportProducts(db, inputJsonProducts);
            //Console.WriteLine(resultInputProducts);

            // => TAKS 3

            //string inputJsonCategories = File.ReadAllText("../../../Datasets/categories.json");
            //string categorInputResults = ImportCategories(db, inputJsonCategories);
            //Console.WriteLine(categorInputResults);

            // => TASK 4

            //string inputJsonCategoryProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            //string categoryProductsResult = ImportCategoryProducts(db, inputJsonCategoryProducts);
            //Console.WriteLine(categoryProductsResult);

            // => TASK 5

            //string jsonProductInRange500to1000 = GetProductsInRange(db);
            //Console.WriteLine(jsonProductInRange500to1000);

            // => TASK 6

            //string usersSoldMoreThanOneProducts = GetSoldProducts(db);
            //Console.WriteLine(usersSoldMoreThanOneProducts);

            // => TASK 7 

            //string categoriesWithRevenue = GetCategoriesByProductsCount(db);
            //Console.WriteLine(categoriesWithRevenue);

            // => TASK 8

            //string getusersWithProductsResult = GetUsersWithProducts(db);
            //Console.WriteLine(getusersWithProductsResult);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var result = context.Users.Where(x => x.ProductsSold.Any(x => x.BuyerId != null))
                .Include(x => x.ProductsSold)
                .ToList()
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Where(x => x.BuyerId != null).Count(),
                        products = x.ProductsSold.Where(x => x.BuyerId != null).Select(x => new
                        {
                            name = x.Name,
                            price = x.Price,
                        })
                    }
                })
                .OrderByDescending(x => x.soldProducts.products.Count());

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var users = new
            {
                usersCount = result.Count(),
                users = result,
            };

            return JsonConvert.SerializeObject(users, Formatting.Indented, settings);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var result = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoriesProducts.Count,
                    averagePrice = $"{x.CategoriesProducts.Average(x => x.Product.Price):f2}",
                    totalRevenue = $"{x.CategoriesProducts.Sum(x => x.Product.Price):f2}",
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var results = context.Users.Where(x => x.ProductsSold.Count > 0 && x.ProductsSold.Any(x => x.Buyer.ProductsBought.Count > 0))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Select(x => new
                    {
                        name = x.Name,
                        price = x.Price,
                        buyerFirstName = x.Buyer.FirstName,
                        buyerLastName = x.Buyer.LastName,
                    })
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList();

            return JsonConvert.SerializeObject(results, Formatting.Indented);
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName,
                })
                .OrderBy(x => x.price)
                .ToList();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);
            return result;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoCategoryProducts = JsonConvert.DeserializeObject<IEnumerable<CategoryProductsImportModel>>(inputJson);
            var categoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(dtoCategoryProducts);
            context.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryImportModel>>(inputJson).Where(x => x.Name != null).ToArray();
            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductImputModel>>(inputJson);
            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            InitializeAutoMapper();

            var dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserImputModel>>(inputJson);  // Parsing the JSON => DTO Collection UserImputModel
            var users = mapper.Map<IEnumerable<User>>(dtoUsers);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}".ToString();
        }

        public static void InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>     // creating mapper configurations using the Profile.cs
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();   // creating mapper with the new configurations
        }
    }

}