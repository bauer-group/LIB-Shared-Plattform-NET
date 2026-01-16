using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{    
    public static class DescriptionHelper
    {                
        public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static string GetDescriptionAttribute(this Enum value)
        {
            return GetAttributeOfType<DescriptionAttribute>(value).Description;
        }
    }     
}
