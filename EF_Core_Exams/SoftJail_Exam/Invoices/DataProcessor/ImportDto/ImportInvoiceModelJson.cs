using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoices.Common;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceModelJson
    {

        [Required]
        [Range(ValidationConstants.InvoiceMinNumber, ValidationConstants.InvoiceMaxNumber)]
        public int Number { get; set; }

        // Better practice when deserializing dates, is better to do it to string, not directly to DATETIME
        [Required]
        public string IssueDate { get; set; } = null!;

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        // When deserializing enumerations it os better to use int not directly to the enumeration type. 
        [Required]
        public int CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }

    }
}
