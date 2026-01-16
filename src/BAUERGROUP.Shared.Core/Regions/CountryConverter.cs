using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace BAUERGROUP.Shared.Core.Regions
{
    public static class CountryConverter
    {
        public static String ISO2ToLocalizedName(String sISO2)
        {            
            var r = new RegionInfo(sISO2);
            return r.DisplayName;
        }

        public static String ISO2ToEnglishName(String sISO2)
        {
            var r = new RegionInfo(sISO2);
            return r.EnglishName;
        }
    }
}
