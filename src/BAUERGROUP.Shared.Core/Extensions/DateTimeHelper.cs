using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class DateTimeHelper
    {
        public static DateTime FromUnixTimestamp(this DateTime oDateTime, double unixTimeStamp, bool bIsUTC = true)
        {            
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);

            return bIsUTC ? dtDateTime.ToUniversalTime() : dtDateTime;
        }

        public static double ToUnixTimestamp(this DateTime oDateTime, bool bIsUTC = true)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return bIsUTC ? (oDateTime.ToUniversalTime() - dtDateTime.ToUniversalTime()).TotalSeconds : (oDateTime.ToLocalTime() - dtDateTime.ToLocalTime()).TotalSeconds;
        }
    }
}
