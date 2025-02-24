using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto
{
    public class ClientModelJson
    {
        public ClientModelJson()
        {
            this.Trucks = new List<int>();
        }

        [JsonProperty("Name")]
        [Required]
        [MinLength(ValidationConstants.ClientNameMinLength)]
        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [Required]
        [MinLength(ValidationConstants.ClientNationalityMinLength)]
        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; }

        [JsonProperty("Trucks")]
        public List<int> Trucks { get; set; }


    }
}


//{
//    "Name": "Kuenehne + Nagel (AG & Co.) KGKuenehne + Nagel (AG & Co.) KGKuenehne + Nagel (AG & Co.) KG",
//    "Nationality": "The Netherlands",
//    "Type": "golden",
//    "Trucks": [
//      1,
//      68,
//      73,
//      17,
//      98,
//      98
//    ]
//  },
