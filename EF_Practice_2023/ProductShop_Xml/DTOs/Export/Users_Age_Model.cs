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
    public class Users_Age_Model
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductModel? SoldProducts { get; set; }

    }
}

// XML

//< Users >
//  < User >
//    < firstName > Almire </ firstName >
//    < lastName > Ainslee </ lastName >
//    < age > 31 </ age >
//    < SoldProducts >
//      < Product >
//        < name > Ampicillin </ name >
//        < price > 674.63 </ price >
//      </ Product >
