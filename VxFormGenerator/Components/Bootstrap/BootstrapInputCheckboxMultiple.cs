using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VxFormGenerator.Components.Plain;

namespace VxFormGenerator.Components.Bootstrap
{
    public class BootstrapInputCheckboxMultiple<TValue> : InputCheckboxMultipleWithChildren<TValue>, IRenderChildren
    {
        public BootstrapInputCheckboxMultiple()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }

        public static new void RenderChildren(RenderTreeBuilder builder,
         int index,
         object dataContext,
         string fieldIdentifier)
        {
            RenderChildren(builder, index, dataContext, fieldIdentifier, typeof(BootstrapInputCheckbox));
        }

    }
}
