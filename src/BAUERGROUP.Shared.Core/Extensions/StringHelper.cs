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
        public static String ReturnEmptyIfNull(this String sInput)
        {
            return (sInput == null) ? String.Empty : sInput;
        }

        public static String RemoveLeadingZeros(this String sInput)
        {
            return sInput.TrimStart('0');
        }

        public static String SetParameterValue(this String sInput, String sParameterName, String sParameterValue, Boolean bEncodeValue2Html = false)
        {
            var sResult = Regex.Replace(sInput, sParameterName, bEncodeValue2Html == false ? sParameterValue : sParameterValue.EncodeToHtml(), RegexOptions.IgnoreCase);
            return sResult;
        }

        public static String EncodeToHtml(this String sInput)
        {
            return WebUtility.HtmlEncode(sInput);
        }

        public static String DecodeFromHtml(this String sInput)
        {
            return WebUtility.HtmlDecode(sInput);
        }

        public static String BinaryToString(this Byte[] bData, Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = new System.Text.UnicodeEncoding();

            return oEncoding.GetString(bData);
        }

        public static Byte[] StringToBinary(this String sInput, Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = new System.Text.UnicodeEncoding();

            return oEncoding.GetBytes(sInput);
        }

        public static Boolean Contains(this String sInput, String sCompare, StringComparison scOption)
        {
            return sInput.IndexOf(sCompare, scOption) >= 0;
        }

        public static String RemoveDelimiter(this String sValue, String sDelimiter)
        {
            return sValue.Replace(sDelimiter, "");
        }

        public static String Truncate(this String sValue, Int16 iMaxLength, Boolean bKeepWholeWords = false)
        {
            if (String.IsNullOrEmpty(sValue))
                return sValue;

            if (bKeepWholeWords)
            {
                if (sValue.Length <= iMaxLength)
                    return sValue;

                var iLastSpacePosition = sValue.LastIndexOf(" ", iMaxLength, StringComparison.Ordinal);
                return sValue.Substring(0, iLastSpacePosition > 0 ? iLastSpacePosition : iMaxLength).TrimEnd();
            }

            return sValue.Length <= iMaxLength ? sValue : sValue.Substring(0, iMaxLength);
        }

        public static Int32 WordsCount(this String sValue)
        {
            if (String.IsNullOrWhiteSpace(sValue))
                return 0;

            return sValue.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static String HTML2Text(this String sValue)
        {
            return Regex.Replace(sValue, "<.*?>", String.Empty);
        }

        public static String RemoveByRegEx(this String sValue, String sRegEx)
        {
            return sValue.ReplaceByRegEx(sRegEx, @"");
        }

        public static String ReplaceByRegEx(this String sValue, String sRegEx, String sReplacement)
        {
            return Regex.Replace(sValue, sRegEx, sReplacement, RegexOptions.IgnoreCase).Trim();
        }

        public static String JoinWithFormat<T>(this IEnumerable<T> oList, String sSeparator, String sFormatString, Boolean bEscapeListForUseInRegularExpression = false)
        {
            var sFormat = String.IsNullOrWhiteSpace(sFormatString) ? @"{0}" : sFormatString;

            if (bEscapeListForUseInRegularExpression)
                return String.Join(sSeparator, oList.Select(i => Regex.Escape(String.Format(sFormat, i))));

            return String.Join(sSeparator, oList.Select(i => String.Format(sFormat, i)));
        }

        public static String HasNullOrWhiteSpaceValue(this String sValue, String sDefaultValue = @"-")
        {
            return String.IsNullOrWhiteSpace(sValue) ? sDefaultValue : sValue;
        }

        public static Boolean IsNullOrWhiteSpace(this String sValue)
        {
            return String.IsNullOrWhiteSpace(sValue);
        }

        public static String RemoveLineBreaks(this String sValue, String sLineBreaksRegEx = @"\t|\n|\r")
        {
            return Regex.Replace(sValue, sLineBreaksRegEx, "");
        }
    }
}
