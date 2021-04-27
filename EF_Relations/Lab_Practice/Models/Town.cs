using System;
using System.Collections.Generic;

namespace Lab_Practice.Models
{
    public class Town
    {
        public Town()
        {
        }

        public int Id { get; set; }
         
        public string Name { get; set; }



        public ICollection<Employee> Locals { get; set; }

        public ICollection<Employee> Workers { get; set; }

    }
}
