using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.CloudinaryClient
{
    public static class CloudinaryExtensions
    {
        public static String GetURL(this Resource oRessource)
        {
            return oRessource.Url.AbsoluteUri;
        }
    }
}
