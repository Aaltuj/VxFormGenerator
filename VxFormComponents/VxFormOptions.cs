using System;
using VxFormGenerator;

namespace VxFormComponents
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
