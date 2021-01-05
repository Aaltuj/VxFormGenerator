using Microsoft.AspNetCore.Components;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Render.Bootstrap
{
    public class VxBootstrapFormRowComponent : OwningComponentBase
    {
        [Parameter] public Core.Layout.VxFormRow FormRowDefinition { get; set; }
        [CascadingParameter] public VxFormLayoutOptions FormLayoutOptions { get; set; }

    }
}

