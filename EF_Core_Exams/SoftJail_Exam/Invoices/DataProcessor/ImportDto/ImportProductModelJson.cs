using Invoices.Common;
using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductModelJson
    {

        [Required]
        [MinLength(ValidationConstants.ProductNameMinLength)]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range((double)ValidationConstants.ProductMinPrice, (double)ValidationConstants.ProductMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryType { get; set; }

        public int[] Clients { get; set; } = null!;

    }
}
