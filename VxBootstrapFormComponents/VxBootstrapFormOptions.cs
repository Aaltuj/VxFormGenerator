using System;
using VxFormGenerator;

namespace VxBootstrapFormComponents
{
    public class VxBootstrapFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public VxBootstrapFormOptions()
        {
            FormElementComponent = typeof(BootstrapFormElement<>);
        }
    }
}
