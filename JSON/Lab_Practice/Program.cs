using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace Lab_Practice
{
    class Program
    {
        static void Main(string[] args)
        {

            //var car = new Car
            //{
            //    Extras = new List<string> { "Klimatronix", "4x4", "Lights" },
            //    ManufacturedOn = DateTime.Now,
            //    Model = "Golf",
            //    Vendor = "WV",
            //    Price = 12000.00M,
            //    Engine = new Engine { Dispacement = 3000, HorsePower = 80 }
            //};

            // File.WriteAllText("myCar.json",JsonSerializer.Serialize(car));  // => creating JSON file


            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };


            var json = File.ReadAllText("myCar.json");

            var car = JsonSerializer.Deserialize<Car>(json);  
             

            Console.WriteLine(JsonSerializer.Serialize(car, options));


        }
    }

    public class Car
    {
        public string Model { get; set; }

        public string Vendor { get; set; }

        public decimal Price { get; set; }

        public DateTime ManufacturedOn { get; set; }

        public List<string> Extras { get; set; }

        public Engine Engine { get; set; }

    }

    public class Engine
    {
        public int HorsePower { get; set; }

        public int Dispacement { get; set; }

    }
}
