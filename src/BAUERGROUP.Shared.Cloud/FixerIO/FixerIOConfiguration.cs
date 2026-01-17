using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    public class FixerIOConfiguration
    {
        public FixerIOConfiguration(String apiKey, Int32 timeout = 3 * 1000, IWebProxy? proxy = null)
        {
            APIKey = apiKey;
            Proxy = proxy;
            Timeout = timeout;
        }

        public FixerIOConfiguration()
            : this(@"")
        {

        }

        public String APIKey { get; private set; }

        public Int32 Timeout { get; private set; }

        public IWebProxy? Proxy { get; private set; }

        public String URL => @"http://data.fixer.io/api/";
    }
}
