using Microsoft.AspNetCore.Components.Forms;
using System;
using VxFormGenerator.Core;
using VxFormGenerator.Form;
using VxFormGenerator.Render.Bulma;
using VxFormGenerator.Render.Plain;

namespace VxFormGenerator.Settings.Bulma
{
    public class VxBulmaFormOptions : IFormGeneratorOptions
    {
        public Type FormElementComponent { get; set; }

        public FieldCssClassProvider FieldCssClassProvider { get; set; }

        public Type FormGroupElement { get; set; }

        public VxBulmaFormOptions()
        {
            FormElementComponent = typeof(BulmaFormElement<>);
            FormGroupElement = typeof(VxFormGroup);
            FieldCssClassProvider = new VxBulmaFormCssClassProvider();
        }
    }
}
