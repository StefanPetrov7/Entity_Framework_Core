using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExPrisonerXmlDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public MessagesXmlDto[] Messages { get; set; }
    }
}

// => XML example output

//< Prisoners >
//  < Prisoner >
//    < Id > 3 </ Id >
//    < Name > Binni Cornhill </ Name >
//    < IncarcerationDate > 1967 - 04 - 29 </ IncarcerationDate >
//    < EncryptedMessages >
//      < Message >
//        < Description > !? sdnasuoht evif - ytnewt rof deksa uoy ro orez artxe na ereht sI</Description> 
//      </Message> 
//    </EncryptedMessages> 
//  </Prisoner> 
//  <Prisoner> 
//    <Id>2</Id> 
//    <Name>Diana Ebbs</Name> 
//    <IncarcerationDate>1963-08-21</IncarcerationDate> 
//    <EncryptedMessages> 
//      <Message> 
//        <Description>.kcab draeh ton evah llits I dna  , skeew 2 tuoba ni si esaeler mubla ehT .dnuoranrut rof skeew 6-4 sekat ynapmoc DC eht dias yllanigiro eH .gnitiaw llits ma I</Description> 
//      </Message> 
//      <Message> 
//        <Description>.emit ruoy ekat ot uoy ekil lliw ew dna krow ruoy ekil I .hsur on emit ruoy ekat , enif si tahT</Description> 
//      </Message> 
//    </EncryptedMessages> 
//  </Prisoner> 
//… 
//</Prisoners> 