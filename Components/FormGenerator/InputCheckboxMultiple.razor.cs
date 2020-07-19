using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class InputCheckboxMultipleComponent<TValue> : VxInputBase<List<ValueReference<TValue, bool>>>
    {
        /// <summary>
        /// Gets or sets the child content to be rendering inside the select element.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <inheritdoc />
        protected override bool TryParseValueFromString(string value, out List<ValueReference<TValue, bool>> result, out string validationErrorMessage)
            => throw new NotImplementedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
    }
}
