using System;
using VxFormGenerator.Core;
using VxFormGenerator.Form;

namespace VxFormGenerator.Settings.Plain
{
    public class VxFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public VxFormOptions()
        {
            FormElementComponent = typeof(FormElement<>);
        }
    }
}
