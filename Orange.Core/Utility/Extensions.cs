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
    }
}
