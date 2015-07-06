using System;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Orange.Core.Utility
{
    public static class Extensions
    {
        #region Enum extension methods
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null) return null;

            FieldInfo field = type.GetField(name);
            if (field == null) return null;

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return (attribute != null) ? attribute.Description : null;
        }
        #endregion

        #region String extension methods
        /// <summary>
        /// Trim a string down to a max number of characters. The string will end with an ellipsis (...).
        /// </summary>
        /// <param name="input">The string being trimmed</param>
        /// <param name="maxCharacters">The max length of the final string.</param>
        /// <returns></returns>
        public static string TrimToEllipsis(this string input, int maxCharacters)
        {
            if (string.IsNullOrWhiteSpace(input)) return "...";
            if (maxCharacters > input.Length) return input + "...";
            return input.Substring(0, maxCharacters - 3) + "...";
        }
        #endregion
    }
}
