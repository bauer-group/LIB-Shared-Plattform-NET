using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    [Serializable]
    public class FixerIODataSymbols : FixerIODataBase
    {
        [JsonPropertyName("symbols")]
        public Dictionary<String, String> Symbols { get; set; }
    }
}
