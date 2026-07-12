using Microsoft.AspNetCore.Components.Forms;
using System;
using VxFormGenerator.Core;
using VxFormGenerator.Form;
using VxFormGenerator.Render.Plain;
using VxFormGenerator.Render.Tailwind;

namespace VxFormGenerator.Settings.Tailwind
{
    public class VxTailwindFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public FieldCssClassProvider FieldCssClassProvider { get; set; }

        public Type FormGroupElement { get; set; }

        public VxTailwindFormOptions()
        {
            FormElementComponent = typeof(TailwindFormElement<>);
            FormGroupElement = typeof(VxFormGroup);
            FieldCssClassProvider = new VxTailwindFormCssClassProvider();
        }
    }
}
