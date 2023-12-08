using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("supplier")]
    public class SupplierExportModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = null!;

        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}

// => Output example .... of the XML 

//< suppliers >

//  < supplier id = "2" name = "Agway Inc." parts - count = "3" />

//  < supplier id = "4" name = "Airgas, Inc." parts - count = "2" />

//  ... 

//</ suppliers > 