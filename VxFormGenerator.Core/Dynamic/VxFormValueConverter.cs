using System;
using System.Globalization;

namespace VxFormGenerator.Core.Dynamic
{
    internal static class VxFormValueConverter
    {
        public static Type GetValueType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static object GetDefaultValue(Type type)
        {
            if (type == typeof(string))
            {
                return string.Empty;
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return null;
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static string FormatValue(object value, Type type)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (GetValueType(type) == typeof(DateTime) && value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public static object ConvertValue(Type type, string value)
        {
            var valueType = GetValueType(type);

            if (valueType == typeof(string))
            {
                return value;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return Nullable.GetUnderlyingType(type) != null ? null : GetDefaultValue(valueType);
            }

            if (valueType == typeof(DateTime))
            {
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }

            if (valueType == typeof(bool))
            {
                return bool.Parse(value);
            }

            return Convert.ChangeType(value, valueType, CultureInfo.InvariantCulture);
        }
    }
}
