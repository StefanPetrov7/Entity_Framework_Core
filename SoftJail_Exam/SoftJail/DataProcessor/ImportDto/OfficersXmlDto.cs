using SoftJail.Data.Models.Enums;
using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficersXmlDto
    {

        [XmlElement("Name")]
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FullName { get; set; } = null!;

        [XmlElement("Money")]
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        [Required]
        [EnumDataType(typeof(Position))]
        [XmlElement("Position")]
        public string Position { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(Weapon))]
        [XmlElement("Weapon")]
        public string Weapon { get; set; } = null!;

        [XmlElement("DepartmentId")]
        [Required]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public PrisonerXmlDto[]? OfficerPrisoners { get; set; }    // Mapping table 

    }
}


//<? xml version = '1.0' encoding = 'UTF-8' ?>
//< Officers >
//    < Officer >
//        < Name > Minerva Kitchingman </ Name >
//        < Money > 2582 </ Money >
//        < Position > Invalid </ Position >
//        < Weapon > ChainRifle </ Weapon >
//        < DepartmentId > 2 </ DepartmentId >
//        < Prisoners >
//            < Prisoner id = "15" />
//        </ Prisoners >
//    </ Officer >
//    < Officer >
//        < Name > Minerva Holl </ Name >
//        < Money > 2582.55 </ Money >
//        < Position > Overseer </ Position >
//        < Weapon > ChainRifle </ Weapon >
//        < DepartmentId > 2 </ DepartmentId >
//        < Prisoners >
//            < Prisoner id = "15" />
//        </ Prisoners >
//    </ Officer >