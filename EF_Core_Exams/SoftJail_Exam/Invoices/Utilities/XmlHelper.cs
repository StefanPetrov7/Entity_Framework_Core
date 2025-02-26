using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName) where T : class

        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);
            using StringReader stringReader = new StringReader(inputXml);
            object? deserializedObject = serializer.Deserialize(stringReader);

            if (deserializedObject == null || deserializedObject is not T deserializedObjectTypes)
            {
                return null;
            }

            return deserializedObjectTypes;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(T), xmlRootAttribute);

            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter = new StringWriter(sb);
            xmlSerialize.Serialize(stringWriter, obj, nameSpaces);

            return sb.ToString().TrimEnd();

        }
    }
}
