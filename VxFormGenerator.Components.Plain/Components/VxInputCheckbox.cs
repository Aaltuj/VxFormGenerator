using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics.CodeAnalysis;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Form.Components.Plain
{
    [VxDataTypeRegistration(SupportedDataType = typeof(bool))]
    public class VxInputCheckbox : VxInputBase<bool>, IDisposable
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string LabelCss { get; set; }
        [Parameter] public string ContainerCss { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var index = 0;

            builder.OpenElement(index, "div");
            builder.AddAttribute(index++, "class", ContainerCss);
            builder.AddContent(index++, new RenderFragment(builder =>
            {
                var index2 = 0;
                builder.OpenElement(index2++, "input");
                builder.AddMultipleAttributes(index2++, AdditionalAttributes);
                builder.AddAttribute(index2++, "type", "checkbox");
                builder.AddAttribute(index2++, "class", CssClass);
                builder.AddAttribute(index2++, "id", Id);
                builder.AddAttribute(index2++, "checked", BindConverter.FormatValue(CurrentValue));
                builder.AddAttribute(index2++, "onchange", EventCallback.Factory.CreateBinder<bool>(this, __value => CurrentValue = __value, CurrentValue));
                builder.CloseElement();

                builder.OpenElement(index2++, "label");
                builder.AddAttribute(index2++, "class", LabelCss);
                builder.AddAttribute(index2++, "for", Id);
                builder.AddContent(index2++, Label);
                builder.CloseElement();
            }));

            builder.CloseElement();
        }

        /// <inheritdoc />
        protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");


    }

}
