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

        public static HTMLUtils FromURI(Uri uri)
        {
            var website = new HtmlWeb();
            return (HTMLUtils)website.Load(uri);
        }

        public static HTMLUtils FromURL(String url)
        {
            var website = new HtmlWeb();
            return (HTMLUtils)website.Load(url);
        }

        public static HTMLUtils FromFile(String filename)
        {
            var html = new HtmlDocument();
            html.Load(filename);
            return (HTMLUtils)html;
        }
    }
}
