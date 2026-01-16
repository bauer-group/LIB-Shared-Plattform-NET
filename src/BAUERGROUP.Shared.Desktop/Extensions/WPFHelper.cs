using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BAUERGROUP.Shared.Desktop.Extensions
{
    public static class WPFHelper
    {
        public static T CloneWPFObject<T>(this T oWPFObject) where T : class
        {
            if (oWPFObject == null)
                return null;

            Object oCloned = null;

            using (var oStream = new MemoryStream())
            {
                XamlWriter.Save(oWPFObject, oStream);
                oStream.Seek(0, SeekOrigin.Begin);
                oCloned = XamlReader.Load(oStream);
            }

            if (oCloned is T)
                return (T)oCloned;

            return null;
        }
    }
}
