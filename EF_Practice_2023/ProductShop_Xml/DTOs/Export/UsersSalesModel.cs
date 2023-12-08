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
    [XmlType("User")]
    public class UsersSalesModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("soldProducts")]
        public ProductsModel[]? SoldProducts { get; set; }

    }
}

// XML

//< Users >
//  < User >
//    < firstName > Almire </ firstName >
//    < lastName > Ainslee </ lastName >
//    < soldProducts >
//      < Product >
//        < name > Ampicillin </ name >
//        < price > 674.63 </ price >
//      </ Product >
