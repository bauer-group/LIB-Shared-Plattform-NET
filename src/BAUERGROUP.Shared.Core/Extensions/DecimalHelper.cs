using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class DecimalHelper
    {
        public static String FormatNumber(this Decimal sValue, DecimalNumberFormat eFormat)
        {
            switch (eFormat)
            {
                case DecimalNumberFormat.Point:
                    return Convert.ToString(sValue, CultureInfo.InvariantCulture.NumberFormat);

                case DecimalNumberFormat.Comma:
                    return Convert.ToString(sValue, CultureInfo.GetCultureInfo("de-DE").NumberFormat);

                default:
                    return Convert.ToString(sValue, CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }

    public enum DecimalNumberFormat
    {
        Point = 0,
        Comma
    }
}
