using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Text;
using VxFormGenerator.Models;

namespace VxFormGenerator.Utils
{
    internal class StringToVxColorConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string stringValue = value as string;

            if (stringValue != null)
            {
                return new VxColor(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is VxColor)
            {
                VxColor obj = value as VxColor;

                ConstructorInfo ctor = typeof(VxColor).GetConstructor(new Type[] { typeof(string) });

                if (ctor != null)
                {
                    return new InstanceDescriptor(ctor, new object[] { obj.Value });
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
