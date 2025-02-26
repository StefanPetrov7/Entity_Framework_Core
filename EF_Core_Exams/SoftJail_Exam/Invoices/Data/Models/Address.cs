using Invoices.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        // This will set the DB column type to NVARCHAR (ClientNameMaxLength) >> Note that only some attributes are taking into consideration when creating the DB.  
        [Required]
        [MaxLength(ValidationConstants.AddressStreetNameMaxLength)]
        public string StreetName { get; set; } = null!;

        [Required]
        public int StreetNumber  { get; set; }

        [Required]
        public string PostCode { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.AddressCityNameMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.AddressCountryNameMaxLength)]
        public string  Country  { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; } = null!;

    }
}
