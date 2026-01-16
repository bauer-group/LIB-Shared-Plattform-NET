using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Utilities
{
    public static class GenericMapping
    {
        public static Dictionary<String, String> GetStringMappingDefinition(List<String> sRawInput, char cSeparator = ',', bool bRemoveEmptyKeys = true)
        {
            Dictionary<String, String> entryMapping = new Dictionary<String, String>();
            foreach (var i in sRawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(cSeparator);
                if (s.Length != 2)
                    throw new FormatException(String.Format("Wrong mapping configuration: {0}", i.Trim()));

                var sKey = s[0].Trim();
                var sValue = s[1].Trim();

                if ((String.IsNullOrWhiteSpace(sKey)) && (bRemoveEmptyKeys))
                    continue;

                try
                {
                    entryMapping.Add(sKey, sValue);
                }
                catch (FormatException ex)
                {
                    throw new FormatException(String.Format("Wrong format in configuration: {0},{1}", sKey, sValue), ex);
                }
            }

            return entryMapping;
        }

        public static String ReplaceFieldContent(Dictionary<String, String> dictionaryMapping, string sInput)
        {
            if (dictionaryMapping.Count == 0)
                return sInput;

            var sReplacementString = dictionaryMapping.Where(p => p.Key.ToUpper() == sInput.ToUpper()).Select(q => q.Value).FirstOrDefault();
            if (sReplacementString == null)
                return sInput;

            return sReplacementString;
        }

        public static Dictionary<Int32, String> GetIntMappingDefinition(List<String> sRawInput, char cSeparator = ',')
        {
            Dictionary<int, string> entryMapping = new Dictionary<int, string>();
            foreach (var i in sRawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(cSeparator);
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

        public static List<Tuple<Int32, String>> GetMultipleIntMappingDefinition(List<String> sRawInput, char cSeparator = ',')
        {
            List<Tuple<int, string>> entryMapping = new List<Tuple<int, string>>();

            foreach (var i in sRawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(cSeparator);
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

        public static List<Tuple<String, String>> GetMultipleStringMappingDefinition(List<String> sRawInput, char cSeparator = ',', bool bRemoveEmptyKeys = true)
        {
            List<Tuple<String, String>> entryMapping = new List<Tuple<String, String>>();

            foreach (var i in sRawInput)
            {
                if (String.IsNullOrWhiteSpace(i))
                    continue;

                var s = i.Trim().Split(cSeparator);
                if (s.Length != 2)
                    throw new FormatException(String.Format("Wrong mapping configuration: {0}", i.Trim()));

                var sKey = s[0].Trim();
                var sValue = s[1].Trim();

                if ((String.IsNullOrWhiteSpace(sKey)) && (bRemoveEmptyKeys))
                    continue;

                try
                {
                    entryMapping.Add(new Tuple<String, String>(sKey, sValue));
                }
                catch (FormatException ex)
                {
                    throw new FormatException(String.Format("Wrong format in configuration: {0},{1}", sKey, sValue, ex));
                }
            }

            return entryMapping;
        }
    }
}
