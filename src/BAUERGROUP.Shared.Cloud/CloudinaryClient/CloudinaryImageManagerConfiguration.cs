using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.CloudinaryClient
{
    public class CloudinaryImageManagerConfiguration
    {
        public CloudinaryImageManagerConfiguration(String name, String apiKey, String apiSecret, String project)
        {
            Name = name;
            APIKey = apiKey;
            APISecret = apiSecret;

            Project = project;
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
