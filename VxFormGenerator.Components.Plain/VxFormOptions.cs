using Microsoft.AspNetCore.Components.Forms;
using System;
using VxFormGenerator.Core;
using VxFormGenerator.Form;

namespace VxFormGenerator.Settings.Plain
{
    public class VxFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }
        public FieldCssClassProvider FieldCssClassProvider { get; set; }
        public Type FormGroupElement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public VxFormOptions()
        {
            FormElementComponent = typeof(FormElement<>);
        }
    }
}
