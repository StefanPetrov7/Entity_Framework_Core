using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class CarsExportModel
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public PartExportModel[]? Parts { get; set; }

    }
}

// => XML output 

//<? xml version = "1.0" encoding = "utf-16" ?>

//< cars >

//  < car make = "Opel" model = "Astra" traveled - distance = "516628215" >

//    < parts >

//      < part name = "Tappet" price = "300.29" />

//      < part name = "Front Left Side Door Glass" price = "100.92" />

//      < part name = "Fan belt" price = "10.99" />

//    </ parts >

//  </ car >

//  ... 

//</ cars >


