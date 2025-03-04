﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("User")]
    public class UserModelXml
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray("Purchases")]
        public  PurchaseXml[] Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
    }
}


//< Users >
//  < User username = "mgraveson" >
//    < Purchases >
//      < Purchase >
//        < Card > 7991 7779 5123 9211 </ Card >
//        < Cvc > 340 </ Cvc >
//        < Date > 2017 - 08 - 31 17:09 </ Date >
//        < Game title = "Counter-Strike: Global Offensive" >
//          < Genre > Action </ Genre >
//          < Price > 12.49 </ Price >
//        </ Game >
//      </ Purchase >
//      < Purchase >
//        < Card > 7790 7962 4262 5606 </ Card >
//        < Cvc > 966 </ Cvc >
//        < Date > 2018 - 02 - 28 08:38 </ Date >
//        < Game title = "Tom Clancy's Ghost Recon Wildlands" >
//          < Genre > Action </ Genre >
//          < Price > 59.99 </ Price >
//        </ Game >
//      </ Purchase >
//    </ Purchases >
//    < TotalSpent > 72.48 </ TotalSpent >
//  </ User > 