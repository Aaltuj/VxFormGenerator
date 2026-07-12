using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using VxFormGenerator.Core;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Tailwind
{
    public class TailwindInputCheckboxMultiple<TValue> : InputCheckboxMultipleWithChildren<TValue>, IRenderChildren
    {
        public TailwindInputCheckboxMultiple()
        {
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "space-y-2" } };
        }

        public new static void RenderChildren(RenderTreeBuilder builder,
         int index,
         object dataContext,
         string fieldIdentifier)
        {
            RenderChildren(builder, index, dataContext, fieldIdentifier, typeof(TailwindInputCheckbox));
        }
    }
}
