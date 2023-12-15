using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class PrisonerXmlDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}


    //< Prisoners >
    //  < Prisoner id = "15" />
    //</ Prisoners > 