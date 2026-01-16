using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class ExceptionHelper
    {
        public static string GetExceptionDetails(this Exception oException)
        {
            var exceptionProperties = oException.GetType().GetProperties();

            var fieldList = exceptionProperties
                             .Select(exceptionProperty => new
                             {
                                 Name = exceptionProperty.Name,
                                 Value = exceptionProperty.GetValue(oException, null)
                             })
                             .Select(x => String.Format(
                                 "{0} = {1}",
                                 x.Name,
                                 x.Value != null ? x.Value.ToString() : String.Empty
                             ));

            return String.Join(Environment.NewLine, fieldList);
        }
    }
}
