using Microsoft.AspNetCore.Components.Forms;
using System;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Core
{
    public interface IFormGeneratorOptions
    {
        /// <summary>
        /// The element that is used to render a wrapped form field
        /// </summary>
        public Type FormElementComponent { get; set; }

        public Type FormGroupElement { get; set; }

        /// <summary>
        /// CSS class provider for custom classes for invalid of valid form field states
        /// </summary>
        public FieldCssClassProvider FieldCssClassProvider { get; set; }
    }
}
