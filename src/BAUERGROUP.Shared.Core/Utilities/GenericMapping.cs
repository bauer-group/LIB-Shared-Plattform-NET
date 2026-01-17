using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    public static class GenericMapping
    {
        public static Dictionary<String, String> GetStringMappingDefinition(List<String> rawInput, char separator = ',', bool removeEmptyKeys = true)
        {
            Dictionary<String, String> entryMapping = new Dictionary<String, String>();
            foreach (var i in rawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(separator);
                if (s.Length != 2)
                    throw new FormatException(String.Format("Wrong mapping configuration: {0}", i.Trim()));

                var key = s[0].Trim();
                var value = s[1].Trim();

                if ((String.IsNullOrWhiteSpace(key)) && (removeEmptyKeys))
                    continue;

                try
                {
                    entryMapping.Add(key, value);
                }
                catch (FormatException ex)
                {
                    throw new FormatException(String.Format("Wrong format in configuration: {0},{1}", key, value), ex);
                }
            }

            return entryMapping;
        }

        public static String ReplaceFieldContent(Dictionary<String, String> dictionaryMapping, string input)
        {
            if (dictionaryMapping.Count == 0)
                return input;

            var replacementString = dictionaryMapping.Where(p => p.Key.ToUpper() == input.ToUpper()).Select(q => q.Value).FirstOrDefault();
            if (replacementString == null)
                return input;

            return replacementString;
        }

        public static Dictionary<Int32, String> GetIntMappingDefinition(List<String> rawInput, char separator = ',')
        {
            Dictionary<int, string> entryMapping = new Dictionary<int, string>();
            foreach (var i in rawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(separator);
                if (s.Length != 2)
                    throw new FormatException(String.Format("Wrong mapping configuration: {0}", i.Trim()));

                try
                {
                    entryMapping.Add(Convert.ToInt32(s[0].Trim()), s[1].Trim());
                }
                catch (FormatException ex)
                {
                    throw new FormatException(String.Format("Wrong format in configuration: {0},{1}", s[0].Trim(), s[1].Trim()), ex);
                }
            }

            return entryMapping;
        }

        public static List<Tuple<Int32, String>> GetMultipleIntMappingDefinition(List<String> rawInput, char separator = ',')
        {
            List<Tuple<int, string>> entryMapping = new List<Tuple<int, string>>();

            foreach (var i in rawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(separator);
                if (s.Length != 2)
                    throw new FormatException(String.Format("Wrong mapping configuration: {0}", i.Trim()));

                try
                {
                    entryMapping.Add(new Tuple<int, string>(Convert.ToInt32(s[0].Trim()), s[1].Trim()));
                }
                catch (FormatException ex)
                {
                    throw new FormatException(String.Format("Wrong format in configuration: {0},{1}", s[0].Trim(), s[1].Trim()), ex);
                }
            }

            return entryMapping;
        }

        public static List<Tuple<String, String>> GetMultipleStringMappingDefinition(List<String> rawInput, char separator = ',', bool removeEmptyKeys = true)
        {
            List<Tuple<String, String>> entryMapping = new List<Tuple<String, String>>();

            foreach (var i in rawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(separator);
                if (s.Length != 2)
                    throw new FormatException(String.Format("Wrong mapping configuration: {0}", i.Trim()));

                var key = s[0].Trim();
                var value = s[1].Trim();

                if ((String.IsNullOrWhiteSpace(key)) && (removeEmptyKeys))
                    continue;

                try
                {
                    entryMapping.Add(new Tuple<String, String>(key, value));
                }
                catch (FormatException ex)
                {
                    throw new FormatException(String.Format("Wrong format in configuration: {0},{1}", key, value, ex));
                }
            }

            return entryMapping;
        }
    }
}
