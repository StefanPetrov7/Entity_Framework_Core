using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        public Despatcher()
        {
            this.Trucks = new HashSet<Truck>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.DespathcerNameMaxLenght)]
        public string Name { get; set; } = null!;

        public string Position { get; set; } = null!;

        [InverseProperty(nameof(Truck.Despatcher))]
        public virtual ICollection<Truck> Trucks { get; set; }
    }
}
