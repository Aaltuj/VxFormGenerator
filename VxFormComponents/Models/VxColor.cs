using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VxFormGenerator.Utils;

namespace VxFormGenerator.Models
{
    [TypeConverter(typeof(StringToVxColorConverter))]
    public class VxColor
    {

        // will contain standard 32bit sRGB (ARGB)
        //
        public string Value { get; private set; }

        public VxColor(string value)
        {
            this.Value = value;
        }
    }
}
