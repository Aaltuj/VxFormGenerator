using Microsoft.AspNetCore.Components.Forms;
using System;
using VxFormGenerator.Core;
using VxFormGenerator.Form;
using VxFormGenerator.Render.Bootstrap;

namespace VxFormGenerator.Settings.Bootstrap
{
    public class VxBootstrapFormOptions : IFormGeneratorOptions
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type FormElementComponent { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public FieldCssClassProvider FieldCssClassProvider { get; set; }
        public Type FormGroupElement { get; set; }

        public VxBootstrapFormOptions()
        {
            FormElementComponent = typeof(BootstrapFormElement<>);
            FormGroupElement = typeof(VxBootstrapFormGroup);
            FieldCssClassProvider = new VxBootstrapFormCssClassProvider();
        }
    }
}
