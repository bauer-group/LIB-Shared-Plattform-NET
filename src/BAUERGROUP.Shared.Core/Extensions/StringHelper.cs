using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class StringHelper
    {
        public static String ReturnEmptyIfNull(this String input)
        {
            return (input == null) ? String.Empty : input;
        }

        public static String RemoveLeadingZeros(this String input)
        {
            return input.TrimStart('0');
        }

        public static String SetParameterValue(this String input, String parameterName, String parameterValue, Boolean encodeValueToHtml = false)
        {
            var result = Regex.Replace(input, parameterName, encodeValueToHtml == false ? parameterValue : parameterValue.EncodeToHtml(), RegexOptions.IgnoreCase);
            return result;
        }

        public static String EncodeToHtml(this String input)
        {
            return WebUtility.HtmlEncode(input);
        }

        public static String DecodeFromHtml(this String input)
        {
            return WebUtility.HtmlDecode(input);
        }

        public static String BinaryToString(this Byte[] data, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = new System.Text.UnicodeEncoding();

            return encoding.GetString(data);
        }

        public static Byte[] StringToBinary(this String input, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = new System.Text.UnicodeEncoding();

            return encoding.GetBytes(input);
        }

        public static Boolean Contains(this String input, String compare, StringComparison option)
        {
            return input.IndexOf(compare, option) >= 0;
        }

        public static String RemoveDelimiter(this String value, String delimiter)
        {
            return value.Replace(delimiter, "");
        }

        public static String Truncate(this String value, Int16 maxLength, Boolean keepWholeWords = false)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            if (keepWholeWords)
            {
                if (value.Length <= maxLength)
                    return value;

                var lastSpacePosition = value.LastIndexOf(" ", maxLength, StringComparison.Ordinal);
                return value.Substring(0, lastSpacePosition > 0 ? lastSpacePosition : maxLength).TrimEnd();
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static Int32 WordsCount(this String value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return 0;

            return value.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static String HTML2Text(this String value)
        {
            return Regex.Replace(value, "<.*?>", String.Empty);
        }

        public static String RemoveByRegEx(this String value, String regEx)
        {
            return value.ReplaceByRegEx(regEx, @"");
        }

        public static String ReplaceByRegEx(this String value, String regEx, String replacement)
        {
            return Regex.Replace(value, regEx, replacement, RegexOptions.IgnoreCase).Trim();
        }

        public static String JoinWithFormat<T>(this IEnumerable<T> list, String separator, String formatString, Boolean escapeListForUseInRegularExpression = false)
        {
            var format = String.IsNullOrWhiteSpace(formatString) ? @"{0}" : formatString;

            if (escapeListForUseInRegularExpression)
                return String.Join(separator, list.Select(i => Regex.Escape(String.Format(format, i))));

            return String.Join(separator, list.Select(i => String.Format(format, i)));
        }

        public static String HasNullOrWhiteSpaceValue(this String value, String defaultValue = @"-")
        {
            return String.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public static Boolean IsNullOrWhiteSpace(this String value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        public static String RemoveLineBreaks(this String value, String lineBreaksRegEx = @"\t|\n|\r")
        {
            return Regex.Replace(value, lineBreaksRegEx, "");
        }
    }
}
