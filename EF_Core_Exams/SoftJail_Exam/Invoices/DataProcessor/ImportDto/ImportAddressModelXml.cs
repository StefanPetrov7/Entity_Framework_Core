using Invoices.Common;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressModelXml
    {
        [XmlElement("StreetName")]
        [Required]
        [MinLength(ValidationConstants.AddressStreetNameMinLength)]
        [MaxLength(ValidationConstants.AddressStreetNameMaxLength)]
        public string StreetName { get; set; } = null!;

        [XmlElement("StreetNumber")]
        [Required]
        public int StreetNumber { get; set; }

        [XmlElement("PostCode")]
        [Required]
        public string PostCode { get; set; } = null!;

        [XmlElement("City")]
        [Required]
        [MinLength(ValidationConstants.AddressCityNameMinLength)]
        [MaxLength(ValidationConstants.AddressCityNameMaxLength)]
        public string City { get; set; } = null!;

        [XmlElement("Country")]
        [Required]
        [MinLength(ValidationConstants.AddressCountryNameMinLength)]
        [MaxLength(ValidationConstants.AddressCountryNameMaxLength)]
        public string Country { get; set; } = null!;

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
