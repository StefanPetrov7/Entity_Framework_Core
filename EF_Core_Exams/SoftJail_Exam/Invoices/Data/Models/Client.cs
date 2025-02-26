using Invoices.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client()
        {
            this.Invoices = new HashSet<Invoice>();
            this.Addresses = new HashSet<Address>();
            this.ProductsClients = new HashSet<ProductClient>();
        }

        [Key]
        public int Id { get; set; }

        // This will set the DB column type to NVARCHAR (ClientNameMaxLength) >> Note that only some attributes are taking into consideration when creating the DB.  
        [Required]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]  
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.ClientVatMaxLength)]
        public string NumberVat  { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; } = null!;

        public virtual ICollection<Address> Addresses  { get; set; } = null!;

        public virtual ICollection<ProductClient> ProductsClients  { get; set; } = null!;

    }
}
