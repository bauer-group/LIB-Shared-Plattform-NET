using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BAUERGROUP.Shared.Core.Extensions
{
    /// <summary>
    /// Object manipulation and JSON serialization helper using System.Text.Json.
    /// </summary>
    public static class ObjectHelper
    {
        private static readonly JsonSerializerOptions _defaultOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        private static readonly JsonSerializerOptions _compactOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Trims all public string properties of the object.
        /// </summary>
        public static void TrimPublicStringProperties(this object obj)
        {
            var properties = obj.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var prop in properties)
            {
                var inputValue = prop.GetValue(obj, null);
                if (inputValue == null)
                {
                    prop.SetValue(obj, string.Empty, null);
                    continue;
                }

                prop.SetValue(obj, ((string)inputValue).Trim(), null);
            }
        }

        /// <summary>
        /// Creates a deep clone of the object using JSON serialization.
        /// </summary>
        public static T? Clone<T>(this T cloneable)
        {
            if (cloneable is null)
                return default;

            var json = JsonSerializer.Serialize(cloneable, _compactOptions);
            return JsonSerializer.Deserialize<T>(json, _compactOptions);
        }

        /// <summary>
        /// Converts an object of type U to type T using JSON serialization.
        /// </summary>
        public static T? ConvertTo<T, U>(this U convertable)
        {
            if (convertable is null)
                return default;

            var json = JsonSerializer.Serialize(convertable, _compactOptions);
            return JsonSerializer.Deserialize<T>(json, _compactOptions);
        }

        /// <summary>
        /// Serializes the object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="indented">Whether to format the output with indentation.</param>
        public static string SerializeToJSON<T>(this T value, bool indented = true)
        {
            var options = indented ? _defaultOptions : _compactOptions;
            return JsonSerializer.Serialize(value, options);
        }

        /// <summary>
        /// Deserializes a JSON string to an object of type T.
        /// </summary>
        public static T? DeserializeFromJSON<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, _defaultOptions);
        }

        /// <summary>
        /// Deserializes a JSON stream to an object of type T.
        /// </summary>
        public static T? DeserializeFromJSON<T>(this Stream jsonStream)
        {
            return JsonSerializer.Deserialize<T>(jsonStream, _defaultOptions);
        }

        /// <summary>
        /// Serializes the object to a JSON stream.
        /// </summary>
        public static void SerializeToJSON<T>(this T value, Stream stream, bool indented = true)
        {
            var options = indented ? _defaultOptions : _compactOptions;
            JsonSerializer.Serialize(stream, value, options);
        }

        /// <summary>
        /// Asynchronously deserializes a JSON stream to an object of type T.
        /// </summary>
        public static async System.Threading.Tasks.ValueTask<T?> DeserializeFromJSONAsync<T>(this Stream jsonStream)
        {
            return await JsonSerializer.DeserializeAsync<T>(jsonStream, _defaultOptions);
        }

        /// <summary>
        /// Asynchronously serializes the object to a JSON stream.
        /// </summary>
        public static async System.Threading.Tasks.Task SerializeToJSONAsync<T>(this T value, Stream stream, bool indented = true)
        {
            var options = indented ? _defaultOptions : _compactOptions;
            await JsonSerializer.SerializeAsync(stream, value, options);
        }

        /// <summary>
        /// Compares all public properties of two objects for equality.
        /// </summary>
        public static bool PropertiesEqual<T>(this T? obj1, T? obj2)
        {
            if (obj1 == null && obj2 == null)
                return true;

            if (obj1 == null || obj2 == null)
                return false;

            var type = obj1.GetType();
            var properties = type
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanRead);

            foreach (var property in properties)
            {
                var value1 = property.GetValue(obj1, null);
                var value2 = property.GetValue(obj2, null);

                if (!Equals(value1, value2))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets all public instance property information for an object.
        /// </summary>
        public static IEnumerable<PropertyInfo> GetPropertyInformations<T>(this T obj)
        {
            if (obj == null)
                return Enumerable.Empty<PropertyInfo>();

            return obj.GetType()
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
