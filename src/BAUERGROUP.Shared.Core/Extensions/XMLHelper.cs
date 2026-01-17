using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class XMLHelper
    {
        public static String? SerializeToXML<T>(this T value)
        {
            if (value == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        public static T? DeserializeFromXML<T>(this String xml)
        {

            if (String.IsNullOrEmpty(xml))
            {
                return default;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    var result = serializer.Deserialize(xmlReader);
                    return result != null ? (T)result : default;
                }
            }
        }

        public static string XMLEscape(this String value)
        {
            //Hint: http://msdn.microsoft.com/de-de/library/system.security.securityelement.escape(v=vs.110).aspx
            return SecurityElement.Escape(value);
        }
    }
}
