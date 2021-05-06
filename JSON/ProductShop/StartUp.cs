using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
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

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var mapConfig = new MapperConfiguration(cfg =>      // the first step of creating mapper. 
            {
                cfg.AddProfile<ProductShopProfile>();           // adding the mapping obj configurations from the Profile inherited class
            });

             mapper = mapConfig.CreateMapper();                 // creating mapper

            string jsonUsers = File.ReadAllText("../../../Datasets/users.json");    // Reading the information from the json file

            var importUsers = ImportUsers(db, jsonUsers);

            Console.WriteLine(importUsers);

        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputModel> dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);    // importing obj from the json file, as new DTO obj for import into the db

            var users = mapper.Map<IEnumerable<User>>(dtoUsers);   // maping the DTO user as user 

            context.Users.AddRange(users);   // importing the users into the db

            context.SaveChanges();

            return $"Successfully imported {users.Count()}";

        }
    }
}