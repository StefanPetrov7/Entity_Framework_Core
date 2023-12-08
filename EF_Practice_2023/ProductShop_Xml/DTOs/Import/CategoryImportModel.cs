using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("Category")]
    public class CategoryImportModel
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;
    }
}

// XML => input model

//< Categories >
//    < Category >
//        < name > Drugs </ name >
//    </ Category >