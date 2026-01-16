using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class LinqHelper
    {
        public static string XMLString(this XElement oElement)
        {
            if (oElement == null)
                return @"";

            return oElement.Value;
        }

        public static Decimal XMLDecimal(this XElement oElement)
        {
            if (oElement == null)
                return 0m;

            return Convert.ToDecimal(oElement.Value);
        }

        public static String PropertyName<T>(this Expression<Func<T, Object>> oField)
        {
            var oLamda = (LambdaExpression)oField;
            if (oLamda.Body is UnaryExpression)
            {
                var ue = (UnaryExpression)(oLamda.Body);
                var me = (MemberExpression)(ue.Operand);
                return ((PropertyInfo)me.Member).Name;
            }
            else
            {
                var me = (MemberExpression)(oLamda.Body);
                return ((PropertyInfo)me.Member).Name;
            }
        }
    }
}
