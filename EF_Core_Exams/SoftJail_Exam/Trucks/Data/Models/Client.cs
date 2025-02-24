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
    public class Client
    {
        public Client()
        {
            this.ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        //  [StringLength(10, MinimumLength = 40)]  // This kind of validations are not met to be done in the DB => EF will not apply it!
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        public string Nationality { get; set; } = null!;  // => = null! is saying to the compilator that this property will not be null, doesn't change code functionality.  

        [Required]
        public string Type { get; set; } = null!;


        [InverseProperty(nameof(ClientTruck.Client))]  // this attribute might give error with JUDGE!
        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
