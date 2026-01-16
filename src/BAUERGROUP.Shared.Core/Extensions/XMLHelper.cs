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
        public static String SerializeToXML<T>(this T oValue)
        {
            if (oValue == null)
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
                    serializer.Serialize(xmlWriter, oValue);
                }
                return textWriter.ToString();
            }
        }

        public static T DeserializeFromXML<T>(this String sXML)
        {

            if (String.IsNullOrEmpty(sXML))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();

            using (StringReader textReader = new StringReader(sXML))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        public static string XMLEscape(this String sValue)
        {
            //Hint: http://msdn.microsoft.com/de-de/library/system.security.securityelement.escape(v=vs.110).aspx
            return SecurityElement.Escape(sValue);
        }
    }
}
