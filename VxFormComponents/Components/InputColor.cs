using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using VxFormGenerator.Core;
using VxFormGenerator.Models;

namespace VxFormGenerator.Form.Components.Plain
{
    public class InputColor: VxInputBase<VxColor>
    {
            /// <inheritdoc />
            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                builder.OpenElement(0, "input");
                builder.AddMultipleAttributes(1, AdditionalAttributes);
                builder.AddAttribute(2, "type", "color");
                builder.AddAttribute(3, "class", CssClass);
                builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<VxColor>(this, __value => CurrentValue = __value, CurrentValue));
                builder.CloseElement();
            }

            /// <inheritdoc />
            protected override bool TryParseValueFromString(string value, out VxColor result, [NotNullWhen(false)] out string validationErrorMessage)
                => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
        }
    
}
