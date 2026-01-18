using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    /// <summary>
    /// Extension methods for retrieving attributes from enum values.
    /// </summary>
    public static class DescriptionHelper
    {
        /// <summary>
        /// Gets an attribute of the specified type from an enum value.
        /// </summary>
        /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The attribute instance.</returns>
        public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        /// <summary>
        /// Gets the description from a <see cref="DescriptionAttribute"/> on an enum value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description text from the attribute.</returns>
        public static string GetDescriptionAttribute(this Enum value)
        {
            return GetAttributeOfType<DescriptionAttribute>(value).Description;
        }
    }
}
