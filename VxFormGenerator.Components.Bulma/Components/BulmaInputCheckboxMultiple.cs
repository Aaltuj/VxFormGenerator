using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using VxFormGenerator.Core;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bulma
{
    public class BulmaInputCheckboxMultiple<TValue> : InputCheckboxMultipleWithChildren<TValue>, IRenderChildren
    {
        public BulmaInputCheckboxMultiple()
        {
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "checkbox" } };
        }

        public new static void RenderChildren(RenderTreeBuilder builder,
         int index,
         object dataContext,
         string fieldIdentifier)
        {
            RenderChildren(builder, index, dataContext, fieldIdentifier, typeof(BulmaInputCheckbox));
        }
    }
}
