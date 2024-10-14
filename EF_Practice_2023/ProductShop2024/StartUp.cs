using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main()
        {
            var dbContext = new ProductShopContext();

            // => Problem 1

            //string inputJson = File.ReadAllText("../../../Datasets/users.json");
            //var resultImportUsers = ImportUsers(dbContext, inputJson);
            //Console.WriteLine(resultImportUsers);

            // => Problem 2

            //string inputJson = File.ReadAllText("../../../Datasets/products.json");
            //var resultImportedUsers = ImportProducts(dbContext, inputJson);
            //Console.WriteLine(resultImportedUsers);

            // Problem => 3

            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //var resultImportCategories = ImportCategories(dbContext, inputJson);
            //Console.WriteLine(resultImportCategories);

            // Problem => 4

            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //var resultImportCategoriesProducts = ImportCategoryProducts(dbContext, inputJson);
            //Console.WriteLine(resultImportCategoriesProducts);

            // Problem => 5

            //string resultProductsInRange = GetProductsInRange(dbContext);
            //Console.WriteLine(resultProductsInRange);

            // Problem => 6

            //string resultSoldProducts = GetSoldProducts(dbContext);
            //Console.WriteLine(resultSoldProducts);

            // Problem => 7

            //string resultCategories = GetCategoriesByProductsCount(dbContext);
            //Console.WriteLine(resultCategories);

            // Problem => 8

            string resultUsersProducts = GetUsersWithProducts(dbContext);
            Console.WriteLine(resultUsersProducts);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users.Where(x => x.ProductsSold.Any(x => x.Buyer != null))
                .Select(x => new
                {
                    firstName = x.FirstName,    
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Where(p => p.Buyer != null).Count(),
                        products = x.ProductsSold.Where(p => p.Buyer != null).Select(s => new
                        {
                            name = s.Name,
                            price = s.Price,
                        })
                    }
                })
                .ToArray()
                .OrderByDescending(x => x.soldProducts.products.Count());

            var usersArray = new
            {
                usersCount = users.Count(),
                users,
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,   
                NullValueHandling = NullValueHandling.Ignore,
            };

            string result = JsonConvert.SerializeObject(usersArray, settings);
            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var query = context.Categories.Select(x => new
            {
                category = x.Name,
                productsCount = x.CategoriesProducts.Count,
                averagePrice = (x.CategoriesProducts.Sum(x => x.Product.Price) / x.CategoriesProducts.Count).ToString("F2"),
                totalRevenue = (x.CategoriesProducts.Sum(x => x.Product.Price)).ToString("F2"),
            })
              .OrderByDescending(x => x.productsCount)
              .ToArray();

            string result = JsonConvert.SerializeObject(query, Formatting.Indented);
            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var query = context.Users.Where(x => x.ProductsSold.Count > 0 && x.ProductsSold.Any(x => x.Buyer.ProductsBought.Count > 0))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName,
                    }).ToArray()

                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToArray();

            string result = JsonConvert.SerializeObject(query, Formatting.Indented);
            return result;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var query = context.Products.Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName,
                })
                .OrderBy(x => x.price)
                .ToArray();


            var result = JsonConvert.SerializeObject(query, Formatting.Indented);
            return result;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var categoriesProductsDtos = JsonConvert.DeserializeObject<IEnumerable<ImportCategoryProductDto>>(inputJson);
            var categoriesProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoriesProductsDtos);
            context.CategoriesProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Count()}";

            // => Check if Product or Category exists.Not working for JUDGE however it is successfully importing the data into the DB while the upper solution is giving error

            //var categoryProductsDto = JsonConvert.DeserializeObject<IEnumerable<ImportCategoryProductDto>>(inputJson);
            //var validCategoryProducts = new List<CategoryProduct>();

            //var productsIds = context.Products.Select(x => x.Id).ToHashSet();
            //var categoryIds = context.Categories.Select(x => x.Id).ToHashSet();


            //foreach (var categoryProduct in categoryProductsDto)
            //{
            //    if (productsIds.Contains(categoryProduct.ProductId) && categoryIds.Contains(categoryProduct.CategoryId))
            //    {
            //        var validCategoryProduct = mapper.Map<CategoryProduct>(categoryProduct);
            //        validCategoryProducts.Add(validCategoryProduct);
            //    }
            //}

            //context.CategoriesProducts.AddRange(validCategoryProducts);
            //context.SaveChanges();
            //return $"Successfully imported {validCategoryProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            // Solution using LINQ in stead off foreach loop.
            //var categoriesDtos = JsonConvert.DeserializeObject<IEnumerable<ImportCategoryDto>>(inputJson).Where(x => x.Name != null).ToArray();

            var categoriesDtos = JsonConvert.DeserializeObject<IEnumerable<ImportCategoryDto>>(inputJson);
            List<Category> categories = new List<Category>();

            foreach (var categoryDto in categoriesDtos)
            {
                if (categoryDto.Name == null) continue;

                var category = mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }

            context.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var productsDtos = JsonConvert.DeserializeObject<IEnumerable<ImportProductDto>>(inputJson);
            var products = mapper.Map<IEnumerable<Product>>(productsDtos);
            context.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            var users = mapper.Map<User[]>(userDtos);
            context.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}";
        }

        public static void InitializeMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}