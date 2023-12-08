using AutoMapper;
using Castle.Core.Resource;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Xml.Serialization;

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

            //string inputXmlUsers = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(db, inputXmlUsers));

            //string inputXmlProducts = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(db, inputXmlProducts));

            //string inputXmlCategories = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(db, inputXmlCategories));

            //string inputCategoriesProducts = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(db, inputCategoriesProducts));

            //Console.WriteLine(GetProductsInRange(db));

            //Console.WriteLine(GetSoldProducts(db));

            //Console.WriteLine(GetCategoriesByProductsCount(db));

            Console.WriteLine(GetUsersWithProducts(db));

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            // No points !!! works fine could be sorting or Judge 

            var usersSoldProducts = context.Users.Where(x => x.ProductsSold.Count > 0)
                .Select(x => new Users_Age_Model
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new SoldProductModel
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.Select(x => new ProductsModel
                        {
                            Name = x.Name,
                            Price = x.Price,
                        }).OrderByDescending(x => x.Price)
                        .ToArray(),
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .Take(10)   
                .ToArray();

            XmlSerializer xmlUserSoldProducts = new XmlSerializer(typeof(Users_Age_Model[]), new XmlRootAttribute("users"));

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            var xmlWriter = new StringWriter();
            xmlUserSoldProducts.Serialize(xmlWriter, usersSoldProducts, nameSpaces);
            return xmlWriter.ToString();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            // No points !!! works fine could be sorting or Judge 

            var categoriesRevenue = context.Categories
                .Select(c => new CategoryExportModel
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Select(x => x.Product).Select(x => x.Price).Sum() / c.CategoryProducts.Count,
                    TotalRevenue = c.CategoryProducts.Select(x => x.Product).Select(x => x.Price).Sum()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            XmlSerializer xmlCategoriesRevenue = new XmlSerializer(typeof(CategoryExportModel[]), new XmlRootAttribute("Categories"));

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            var xmlWriter = new StringWriter();
            xmlCategoriesRevenue.Serialize(xmlWriter, categoriesRevenue, nameSpaces);
            return xmlWriter.ToString();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            // No points !!! works fine could be sorting or Judge 

            var soldProductsQuery = context.Users.Where(x => x.ProductsSold.Count > 0)
                .Select(x => new UsersSalesModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(x => new ProductsModel
                    {
                        Name = x.Name,
                        Price = x.Price,
                    }).ToArray()
                })
                .Take(5)
                .ToArray();

            XmlSerializer xmlUsersSoldProducts = new XmlSerializer(typeof(UsersSalesModel[]), new XmlRootAttribute("Products"));

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            var xmlWriter = new StringWriter();
            xmlUsersSoldProducts.Serialize(xmlWriter, soldProductsQuery, nameSpaces);
            return xmlWriter.ToString();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsQuery = context.Products.Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ProductsInRange_500_100_ExportModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName,
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            XmlSerializer xmlProducts = new XmlSerializer(typeof(ProductsInRange_500_100_ExportModel[]), new XmlRootAttribute("Products"));

            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            var xmlWriter = new StringWriter();
            xmlProducts.Serialize(xmlWriter, productsQuery, nameSpaces);
            return xmlWriter.ToString();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            CreateMapperInstance();

            var categoryProductsImputXml = new XmlSerializer(typeof(CategoryProductImportModel[]), new XmlRootAttribute("CategoryProducts"));
            var xmlReader = new StringReader(inputXml);
            var categoryProductsDtos = categoryProductsImputXml.Deserialize(xmlReader) as CategoryProductImportModel[];
            categoryProductsDtos = categoryProductsDtos.Where(x => x.CategoryId != null && x.ProductId != null).ToArray();
            var categoriesProducts = mapper.Map<CategoryProduct[]>(categoryProductsDtos);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            CreateMapperInstance();

            var categoryImputXml = new XmlSerializer(typeof(CategoryImportModel[]), new XmlRootAttribute("Categories"));
            var xmlReader = new StringReader(inputXml);
            var categoryDtos = categoryImputXml.Deserialize(xmlReader) as CategoryImportModel[];
            categoryDtos = categoryDtos.Where(x => x.Name != null).ToArray();
            var categories = mapper.Map<Category[]>(categoryDtos);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            CreateMapperInstance();

            var productImputXml = new XmlSerializer(typeof(ProductImportModel[]), new XmlRootAttribute("Products"));
            var xmlReader = new StringReader(inputXml);
            var productsDto = productImputXml.Deserialize(xmlReader) as ProductImportModel[];
            var products = mapper.Map<Product[]>(productsDto);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            CreateMapperInstance();

            var usersImputXml = new XmlSerializer(typeof(UsersImportModel[]), new XmlRootAttribute("Users"));
            var xmlReader = new StringReader(inputXml);
            var usersDto = usersImputXml.Deserialize(xmlReader) as UsersImportModel[];
            var users = mapper.Map<User[]>(usersDto);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static void CreateMapperInstance()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}