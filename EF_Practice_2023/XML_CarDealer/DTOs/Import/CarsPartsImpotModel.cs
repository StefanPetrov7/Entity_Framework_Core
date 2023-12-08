using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("partId")]
    public class CarsPartsImpotModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}

//  <partId id="38"/>   // => part of the array in the XML element 
