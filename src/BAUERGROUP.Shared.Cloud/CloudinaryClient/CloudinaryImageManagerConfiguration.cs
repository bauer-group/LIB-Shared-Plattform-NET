using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.CloudinaryClient
{
    public class CloudinaryImageManagerConfiguration
    {
        public CloudinaryImageManagerConfiguration(String sName, String sAPIKey, String sAPISecret, String sProject)
        {
            Name = sName;
            APIKey = sAPIKey;
            APISecret = sAPISecret;

            Project = sProject;
        }

        public CloudinaryImageManagerConfiguration()
            : this(@"", @"", @"", @"")
        {

        }

        public String Name { get; set; }
        public String APIKey { get; set; }
        public String APISecret { get; set; }

        public String Project { get; set; }
    }
}
