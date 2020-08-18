
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace VxFormGenerator.Components.Bootstrap
{
    public class BootstrapInputCheckboxComponent : VxInputBase<bool>
    {
        [Parameter] public string Label { get; set; }

        protected override bool TryParseValueFromString(string value, out bool result, out string validationErrorMessage)
           => throw new NotImplementedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
    }

}
