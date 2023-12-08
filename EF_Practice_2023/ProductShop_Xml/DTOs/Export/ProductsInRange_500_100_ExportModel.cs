using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("Product")]
    public class ProductsInRange_500_100_ExportModel
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public string Buyer { get; set; } = null!;
    }
}

// XML = Model

//<? xml version = "1.0" encoding = "utf-16" ?>
//< Products >
//  < Product >
//    < name > TRAMADOL HYDROCHLORIDE </ name >
//    < price > 516.48 </ price >
//    < buyer > Jacquelin Fransoni </ buyer >
//  </ Product > 