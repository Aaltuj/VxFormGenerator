using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace VxFormGenerator.Components
{
    public class BootstrapInputCheckboxMultiple<TValue> : InputCheckboxMultipleWithOptions<TValue>, IRenderChildren
    {

        public BootstrapInputCheckboxMultiple()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }

        public static new void RenderChildren(RenderTreeBuilder builder,
         int index,
         object dataContext,
         PropertyInfo propInfoValue)
        {
            RenderChildren(builder, index, dataContext, propInfoValue, typeof(BootstrapInputCheckbox));
        }


    }
}
