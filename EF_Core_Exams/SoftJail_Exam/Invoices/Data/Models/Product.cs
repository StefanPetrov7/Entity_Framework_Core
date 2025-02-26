using Invoices.Common;
using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        // This will set the DB column type to NVARCHAR (ClientNameMaxLength) >> Note that only some attributes are taking into consideration when creating the DB.  
        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        // For nav property it is better to use HashSet (avoiding duplications) and to create the instance, EF not EF core will create it for us. 
        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();


    }
}

