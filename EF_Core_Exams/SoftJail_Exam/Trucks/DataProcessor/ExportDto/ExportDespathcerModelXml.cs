using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespathcerModelXml
    {
        public ExportDespathcerModelXml()
        {
            this.Trucks = new List<ExportTruckModelXml>();    
        }

        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;

        [XmlArray("Trucks")]
        public List<ExportTruckModelXml> Trucks { get; set; }
    }
}
