using System;
using VxFormGenerator.Core;
using VxFormGenerator.Form;

namespace VxFormGenerator.Settings.Bootstrap
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
