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

        /// <summary>
        /// Deserializes a JSON string to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized object, or null if deserialization fails.</returns>
        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        /// <summary>
        /// Serializes an object to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <returns>The JSON string representation of the object.</returns>
        public static string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        /// <summary>
        /// Serializes an object to a stream as JSON.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <param name="stream">The stream to write to.</param>
        public static void Serialize<T>(T value, Stream stream)
        {
            JsonSerializer.Serialize(stream, value, _options);
        }

        /// <summary>
        /// Deserializes JSON from a stream to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="stream">The stream containing the JSON data.</param>
        /// <returns>The deserialized object, or null if deserialization fails.</returns>
        public static T? Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, _options);
        }
    }
}
