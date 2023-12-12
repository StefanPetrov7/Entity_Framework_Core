using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Models
{
    [XmlType("Tag")]
    public class TagInfoModel
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;
    }
}
