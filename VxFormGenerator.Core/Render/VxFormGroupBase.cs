using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Core.Render
{
    public class VxFormGroupBase : OwningComponentBase
    {
        [Parameter] public Layout.VxFormGroup FormGroupDefinition { get; set; }
    }
}

