using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientModelXml
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        [XmlElement("ClientName")]
        public string ClientName { get; set; } = null!;

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; } = null!;

        [XmlArray("Invoices")]
        public ExportInvoiceModelXml[] Invoices { get; set; } = null!;
    }
}
