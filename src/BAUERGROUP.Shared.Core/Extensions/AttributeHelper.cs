using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class AttributeHelper
    {
        //Useage: var r = typeof(MyClass).GetAttributeValue((MyAttribute a) => a.Name);
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var r = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            
            if (r != null)
            {
                return valueSelector(r);
            }

            return default(TValue);            
        }        
    }
}
