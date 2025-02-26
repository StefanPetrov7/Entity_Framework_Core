using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Invoice")]
    public class ExportInvoiceModelXml
    {
        [XmlElement("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

        [XmlElement("InvoiceAmount")]
        public string InvoiceAmount { get; set; } = null!;

        [XmlIgnore]
        public DateTime ForSortingIssueDate { get; set; } 

        [XmlIgnore]
        public DateTime ForSortingDueDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; } = null!;

        [XmlElement("Currency")]
        public string Currency { get; set; } = null!;
    }
}
