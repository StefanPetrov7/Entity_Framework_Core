using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Models
{
    /// <summary>
    /// XML model output
    /// </summary>

    [XmlType("PropertyInfo")]
    public class PropertyFullDataModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("districtName")]
        public string DistrictName { get; set; } = null!;

        [XmlElement("size")]
        public int Size { get; set; }

        [XmlElement("price")]
        public int Price { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("propertyTYpe")]
        public string PropertyType { get; set; } = null!;

        [XmlElement("buildigType")]
        public string BuildingType { get; set; } = null!;

        [XmlArray("tags")]
        public TagInfoModel[]? Tags { get; set; }

    }
}