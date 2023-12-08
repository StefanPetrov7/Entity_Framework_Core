using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("Category")]
    public class CategoryExportModel
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}

// XML => Model

//< Categories >
//  < Category >
//    < name > Garden </ name >
//    < count > 23 </ count >
//    < averagePrice > 800.150869 </ averagePrice >
//    < totalRevenue > 18403.47 </ totalRevenue >
//  </ Category >
