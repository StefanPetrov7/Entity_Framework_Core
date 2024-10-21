using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string input, string rootName)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            using StringReader stringReader = new StringReader(input);

            T returnT = (T)xmlSerializer.Deserialize(stringReader);

            return returnT;
        }

        public IEnumerable<T> DeserializeCollection<T>(string input, string rootName)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            using StringReader stringReader = new StringReader(input);

            IEnumerable<T> returnT = (IEnumerable<T>)xmlSerializer.Deserialize(stringReader);

            return returnT;
        }
    }
}

