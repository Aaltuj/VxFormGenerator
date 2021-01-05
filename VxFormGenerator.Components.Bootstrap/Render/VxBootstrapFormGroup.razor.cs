using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Render.Bootstrap
{
    public class VxBootstrapFormGroupComponent : OwningComponentBase
    {
        [Parameter] public Core.Layout.VxFormGroup FormGroupDefinition { get; set; }

        [CascadingParameter] public VxFormLayoutOptions FormLayoutOptions { get; set; }
    }
}

