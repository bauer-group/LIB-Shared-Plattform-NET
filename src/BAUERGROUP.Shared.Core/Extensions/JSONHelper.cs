using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BAUERGROUP.Shared.Core.Extensions
{
    /// <summary>
    /// JSON serialization helper using System.Text.Json.
    /// </summary>
    [Obsolete("Use ObjectHelper.SerializeToJSON/DeserializeFromJSON instead")]
    public static class JSONHelper
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        public static string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        public static void Serialize<T>(T value, Stream stream)
        {
            JsonSerializer.Serialize(stream, value, _options);
        }

        public static T? Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, _options);
        }
    }
}
