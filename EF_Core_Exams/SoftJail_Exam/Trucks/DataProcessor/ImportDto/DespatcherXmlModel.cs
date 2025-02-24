using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;
using Trucks.Data.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class DespatcherXmlModel
    { 
        public DespatcherXmlModel()
        {
            this.Trucks = new List<TruckModelXml>();
        }

        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstants.DespathcerNameMinLenght)]
        [MaxLength(ValidationConstants.DespathcerNameMaxLenght)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        [Required]
        public string? Position { get; set; } = null!;

        [XmlArray("Trucks")]
        public List<TruckModelXml> Trucks { get; set; }
    }
}


    //< Despatcher >
    //    < Name > Genadi Petrov </ Name >
    //    < Position > Specialist </ Position >
    //    < Trucks >
    //        < Truck >
    //            < RegistrationNumber > CB0796TP </ RegistrationNumber >
    //            < VinNumber > YS2R4X211D5318181 </ VinNumber >
    //            < TankCapacity > 1000 </ TankCapacity >
    //            < CargoCapacity > 23999 </ CargoCapacity >
    //            < CategoryType > 0 </ CategoryType >
    //            < MakeType > 3 </ MakeType >
    //        </ Truck >
    //        < Truck >
    //            < RegistrationNumber > CB0818TP </ RegistrationNumber >
    //            < VinNumber > YS2R4X211D5318128 </ VinNumber >
    //            < TankCapacity > 1400 </ TankCapacity >
    //            < CargoCapacity > 29004 </ CargoCapacity >
    //            < CategoryType > 3 </ CategoryType >
    //            < MakeType > 0 </ MakeType >
    //        </ Truck >
    //    </ Trucks >
    //</ Despatcher >
