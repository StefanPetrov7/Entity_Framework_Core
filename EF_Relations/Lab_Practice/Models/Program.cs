using System;
using System.Collections.Generic;
using System.Linq;
using Lab_Practice.Models;

namespace Lab_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            var query = db.Employees.Join
                (
                db.Employees,
                x => x.AddressId,
                x => x.AddressId,
                (employees, addresses) => new
                {
                    FirstName = employees.FirstName,
                    Text = addresses.Address.AddressText,

                }
                ) ;



            foreach (var kvp in query)
            {
                Console.Write(kvp.FirstName );
                Console.Write( kvp.Text);
                Console.WriteLine();

            }
            

        }
    }
}
