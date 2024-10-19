using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Supplier")]
    public class ImportSupplierModelXml
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("isImported")]
        public bool IsImported { get; set; }
    }
}

