using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckModelXml
    {
        private int _categoryType;

        [XmlElement("RegistrationNumber")]
        [MinLength(ValidationConstants.TruckRegistrationNumberLenght)]
        [MaxLength(ValidationConstants.TruckRegistrationNumberLenght)]
        [RegularExpression(ValidationConstants.TruckRegistrationNumberRegEx)]
        public string? RegistrationNumber { get; set; } = null!;

        [XmlElement("VinNumber")]
        [Required]
        [MinLength(ValidationConstants.TruckVinNumberNumberLenght)]
        [MaxLength(ValidationConstants.TruckVinNumberNumberLenght)]
        public string VinNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }
         
        [XmlElement("CategoryType")]
        [Range(0,3)]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        [Range(0, 4)]
        public int MakeType { get; set; }

    }
}


//< Despatcher >  ----> Root
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
