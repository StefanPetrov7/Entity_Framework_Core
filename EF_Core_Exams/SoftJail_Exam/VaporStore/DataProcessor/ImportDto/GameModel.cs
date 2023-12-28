using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class GameModel
    {
        public GameModel()
        {
            this.Tags = new List<string>();  
        }

        [Required]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public  string Genre { get; set; }

        public  List<string> Tags { get; set; }
    }
}

