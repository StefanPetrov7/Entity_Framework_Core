using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using System.Xml.Serialization;
using System.Globalization;

namespace VaporStore.DataProcessor.ImportDto
{

    [XmlType("Purchase")]
    public class PurchaseModel
    {
        [Required]
        [XmlAttribute("title")]
        public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(PurchaseType))]
        [XmlElement("Type")]
        public PurchaseType? Type { get; set; }

        [Required]
        [XmlElement("Key")]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string ProductKey { get; set; }

        [Required]
        [XmlElement("Card")]
        [RegularExpression(@"^\d{4}\s\d{4}\s\d{4}\s\d{4}$")]
        public string Card { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
   
    }
}

