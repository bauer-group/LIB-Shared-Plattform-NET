using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    [Serializable]
    public class FixerIODataBase
    {
        [JsonPropertyName("success")]
        public Boolean Success { get; set; }

        [JsonPropertyName("error")]
        public ErrorInfo Error { get; set; }

        public class ErrorInfo
        {
            [JsonPropertyName("code")]
            public Int32 Code { get; set; }

            [JsonPropertyName("type")]
            public String Type { get; set; }

            [JsonPropertyName("info")]
            public String Info { get; set; }
        }
    }
}