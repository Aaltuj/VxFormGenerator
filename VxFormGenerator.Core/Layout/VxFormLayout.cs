using System;
using System.Collections.Generic;
using System.Text;

namespace VxFormGenerator.Core.Layout
{
    public class VxFormLayoutAttribute
    {
        public string Label { get; set; }

        public List<VxFormGroup> Groups { get; set; }

    }
}
