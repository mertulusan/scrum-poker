using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ScrumPoker.Model
{
    public static class Extensions
    {
        public static string GetEnumDescription<T>(this T value)
        {
            if (value == null ||
                !System.Enum.IsDefined(value.GetType(), value))
                return string.Empty;

            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;

            return value.ToString();
        }
    }
}
