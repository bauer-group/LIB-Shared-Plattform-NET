using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class UrlHelper
    {
        public static String SetURLParameters<T>(this T oData, String sURL)
        {            
            var oProperties = oData.GetPropertyInformations();
            foreach (var oProperty in oProperties)
            {
                var oValue = oProperty.GetValue(oData, null);
                sURL = sURL.SetParameterValue($"{{{oProperty.Name}}}", oValue == null ? "" : (string)oValue, true);
            }

            return sURL;
        }
    }
}
