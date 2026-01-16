using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Internet
{
    public class HTMLUtils : HtmlDocument
    {
        public HTMLUtils()
            : base()
        {

        }

        public static HTMLUtils FromURI(Uri oUri)            
        {
            var oWebsite = new HtmlWeb();
            return (HTMLUtils)oWebsite.Load(oUri);
        }

        public static HTMLUtils FromURL(String sURL)
        {
            var oWebsite = new HtmlWeb();
            return (HTMLUtils)oWebsite.Load(sURL);
        }

        public static HTMLUtils FromFile(String sFilename)
        {
            var oHTML = new HtmlDocument();
            oHTML.Load(sFilename);
            return (HTMLUtils)oHTML;
        }
    }
}
