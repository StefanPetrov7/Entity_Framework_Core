using Invoices.Common;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientModelXml
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstants.ClientNameMinLength)]
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement("NumberVat")]
        [Required]
        [MinLength(ValidationConstants.ClientVatMinLength)]
        [MaxLength(ValidationConstants.ClientVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        public ImportAddressModelXml[] Addresses { get; set; } = null!;


    }
}


//<? xml version = "1.0" encoding = "UTF-8" ?>
//< Clients >
//  < Client >
//    < Name > LiCB </ Name >
//    < NumberVat > BG5464156654654654 </ NumberVat >
//    < Addresses >
//      < Address >
//        < StreetName > Gnigler strasse </ StreetName >
//        < StreetNumber > 57 </ StreetNumber >
//        < PostCode > 5020 </ PostCode >
//        < City > Salzburg </ City >
//        < Country > Austria </ Country >
//      </ Address >
//    </ Addresses >
//  </ Client >
//  < Client >
//    < Name > Mr.Pen </ Name >
//    < NumberVat > BG5464156654654654 </ NumberVat >
//    < Addresses >
//      < Address >
//        < StreetName > Gewerbestrasse </ StreetName >
//        < StreetNumber > 12 </ StreetNumber >
//        < PostCode > 5165 </ PostCode >
//        < City > Berndorf bei Salzburg</City>
//        <Country>Austria</Country>
//      </Address>
//    </Addresses>
//  </Client>