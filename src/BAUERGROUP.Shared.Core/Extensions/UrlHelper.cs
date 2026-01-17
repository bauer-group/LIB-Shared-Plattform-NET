using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class UrlHelper
    {
        public static String SetURLParameters<T>(this T data, String url)
        {
            var properties = data.GetPropertyInformations();
            foreach (var property in properties)
            {
                var value = property.GetValue(data, null);
                url = url.SetParameterValue($"{{{property.Name}}}", value == null ? "" : Uri.EscapeDataString((string)value), false);
            }

            return url;
        }
    }
}
