using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("Product")]
    public class ProductImportModel
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("sellerId")]
        public int SellerId { get; set; }

        [XmlElement("buyerId")]
        public int? BuyerId { get; set; }
    }
}

// XML => model

  //< Product >
  //      < name > SNORING HP </ name >
  //      < price > 53.59 </ price >
  //      < sellerId > 24 </ sellerId >
  //      < buyerId > 30 </ buyerId >
  //  </ Product >


