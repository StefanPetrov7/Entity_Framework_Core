using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Customer")]
    public class ImportCustomerModelXml
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        // => It is better to take the DATE as string and then parse it to DATETIME to ensure that no errors
        // => Same is valid when we working with ENUM types etc...

        [XmlElement("birthDate")]
        public string BirthDate { get; set; } = null!;

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}

