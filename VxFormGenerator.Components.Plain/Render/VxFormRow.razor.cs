using Microsoft.AspNetCore.Components;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Render.Plain
{
    public class VxFormRowComponent : OwningComponentBase
    {
        [Parameter] public Core.Layout.VxFormRow FormRowDefinition { get; set; }
        [CascadingParameter] public VxFormLayoutOptions FormLayoutOptions { get; set; }

    }
}

