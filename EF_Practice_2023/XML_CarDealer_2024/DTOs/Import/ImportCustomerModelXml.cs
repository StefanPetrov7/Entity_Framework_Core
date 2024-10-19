using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Customer")]
    public class ImportCustomerModelXml
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        // => It is better to take the date as string and then parse it to DATETIME to ensure that no errors 

        [XmlElement("birthDate")]
        public string BirthDate { get; set; } = null!;

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}

