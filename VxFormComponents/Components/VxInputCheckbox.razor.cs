using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using VxFormGenerator;

namespace VxFormComponents.Components
{
    public class VxInputCheckboxComponent : VxInputBase<bool>, IDisposable
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string LabelCss { get; set; }
        [Parameter] public string ContainerCss { get; set; }

        protected override bool TryParseValueFromString(string value, out bool result, out string validationErrorMessage)
           => throw new NotImplementedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");


    }

}
