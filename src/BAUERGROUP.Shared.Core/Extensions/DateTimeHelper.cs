using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class DateTimeHelper
    {
        public static DateTime FromUnixTimestamp(this DateTime dateTime, double unixTimeStamp, bool isUTC = true)
        {
            DateTime epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            epochDateTime = epochDateTime.AddSeconds(unixTimeStamp);

            return isUTC ? epochDateTime.ToUniversalTime() : epochDateTime;
        }

        public static double ToUnixTimestamp(this DateTime dateTime, bool isUTC = true)
        {
            DateTime epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return isUTC ? (dateTime.ToUniversalTime() - epochDateTime.ToUniversalTime()).TotalSeconds : (dateTime.ToLocalTime() - epochDateTime.ToLocalTime()).TotalSeconds;
        }
    }
}
