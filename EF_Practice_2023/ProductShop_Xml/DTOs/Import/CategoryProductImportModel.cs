using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("CategoryProduct")]
    public class CategoryProductImportModel
    {
        [XmlElement("CategoryId")]
        public int CategoryId { get; set; }

        [XmlElement("ProductId")]
        public int ProductId { get; set; }
    }
}
// XML => model

//< CategoryProducts >
//    < CategoryProduct >
//        < CategoryId > 4 </ CategoryId >
//        < ProductId > 1 </ ProductId >
//    </ CategoryProduct >
