﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class MessagesXmlDto
    {
        [XmlElement("Description")]
        public  string Description { get; set; }

     
    }
}
