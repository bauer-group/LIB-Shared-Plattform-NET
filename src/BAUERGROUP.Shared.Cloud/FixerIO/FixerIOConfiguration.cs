using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    public class FixerIOConfiguration
    {
        public FixerIOConfiguration(String sAPIKey, Int32 iTimeout = 3 * 1000, IWebProxy oProxy = null)
        {
            APIKey = sAPIKey;
            Proxy = oProxy;
            Timeout = iTimeout;
        }

        public FixerIOConfiguration()
            : this(@"")
        {

        }

        public String APIKey { get; private set; }

        public Int32 Timeout { get; private set; }

        public IWebProxy Proxy { get; private set; }

        public String URL => @"http://data.fixer.io/api/";
    }
}
