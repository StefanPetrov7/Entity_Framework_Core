using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public Truck()
        {
            this.ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.TruckRegistrationNumberLenght)]
        public string? RegistrationNumber { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TruckVinNumberNumberLenght)]
        public string VinNumber { get; set; } = null!;

        // [Range(950, 1420)] // => this validation will be done when importing the data
        public int TankCapacity { get; set; }

        // [Range(5000, 29000)] // => this validation will be done when importing the data
        public int CargoCapacity { get; set; }

        [Required]  // enum type is required by default as it is numeric type data
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }
        public virtual Despatcher Despatcher { get; set; } = null!;


        [InverseProperty(nameof(ClientTruck.Truck))]
        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }


    }
}


