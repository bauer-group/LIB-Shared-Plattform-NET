using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    [Serializable]
    public class FixerIODataRates : FixerIODataBase
    {
        [JsonPropertyName("timestamp")]
        public Int32 Timestamp { get; set; }

        [JsonPropertyName("base")]
        public String Base { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<String, Decimal> Rates { get; set; }
    }
}
