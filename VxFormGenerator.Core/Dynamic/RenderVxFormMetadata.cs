using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace VxFormGenerator.Core.Dynamic
{
    public sealed class RenderVxFormMetadata : ComponentBase
    {
        [Parameter]
        public VxFormMetadataModel Model { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Model == null)
            {
                return;
            }

            var sequence = 0;

            foreach (var row in Model.Fields
                .Where(IsVisible)
                .OrderBy(field => field.RowId.GetValueOrDefault())
                .ThenBy(field => field.Order.GetValueOrDefault())
                .GroupBy(field => field.RowId.GetValueOrDefault()))
            {
                builder.OpenElement(sequence++, "div");
                builder.AddAttribute(sequence++, "class", "vx-form-row row");
                builder.AddAttribute(sequence++, "data-row-id", row.Key);

                var rowLabel = row.Select(field => field.RowLabel).FirstOrDefault(label => !string.IsNullOrWhiteSpace(label));
                if (!string.IsNullOrWhiteSpace(rowLabel))
                {
                    builder.OpenElement(sequence++, "div");
                    builder.AddAttribute(sequence++, "class", "vx-form-row-label col-12");
                    builder.AddContent(sequence++, rowLabel);
                    builder.CloseElement();
                }

                foreach (var field in row.OrderBy(field => field.Order.GetValueOrDefault()).ThenBy(field => field.Name))
                {
                    builder.OpenElement(sequence++, "div");
                    builder.AddAttribute(sequence++, "class", GetFieldClass(field));

                    builder.OpenElement(sequence++, "label");
                    builder.AddAttribute(sequence++, "for", field.Id);
                    builder.AddContent(sequence++, string.IsNullOrWhiteSpace(field.Label) ? field.Name : field.Label);
                    builder.CloseElement();

                    if (GetFieldKind(field) == VxFormFieldKind.Select)
                    {
                        RenderSelect(builder, ref sequence, field);
                    }
                    else
                    {
                        RenderInput(builder, ref sequence, field);
                    }

                    if (!string.IsNullOrWhiteSpace(field.Description))
                    {
                        builder.OpenElement(sequence++, "small");
                        builder.AddAttribute(sequence++, "class", "form-text text-muted");
                        builder.AddContent(sequence++, field.Description);
                        builder.CloseElement();
                    }

                    builder.CloseElement();
                }

                builder.CloseElement();
            }
        }

        private void RenderSelect(RenderTreeBuilder builder, ref int sequence, VxFormFieldMetadata field)
        {
            var value = VxFormValueConverter.FormatValue(Model.Values[field.Name], field.FieldType);

            builder.OpenElement(sequence++, "select");
            builder.AddAttribute(sequence++, "id", field.Id);
            builder.AddAttribute(sequence++, "name", field.Name);
            builder.AddAttribute(sequence++, "class", "form-control");
            builder.AddAttribute(sequence++, "value", value);
            builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.CreateBinder<string>(this, selectedValue => SetValue(field, ConvertValue(field, selectedValue)), value));

            foreach (var option in field.Options)
            {
                builder.OpenElement(sequence++, "option");
                builder.AddAttribute(sequence++, "value", option.Value);

                if (option.IsDisabled)
                {
                    builder.AddAttribute(sequence++, "disabled", true);
                }

                builder.AddContent(sequence++, string.IsNullOrWhiteSpace(option.Label) ? option.Value : option.Label);
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        private void RenderInput(RenderTreeBuilder builder, ref int sequence, VxFormFieldMetadata field)
        {
            builder.OpenElement(sequence++, "input");
            builder.AddAttribute(sequence++, "id", field.Id);
            builder.AddAttribute(sequence++, "name", field.Name);
            builder.AddAttribute(sequence++, "class", GetFieldKind(field) == VxFormFieldKind.Checkbox ? "form-check-input" : "form-control");
            builder.AddAttribute(sequence++, "type", GetInputType(field));

            if (!string.IsNullOrWhiteSpace(field.Placeholder))
            {
                builder.AddAttribute(sequence++, "placeholder", field.Placeholder);
            }

            if (field.IsRequired)
            {
                builder.AddAttribute(sequence++, "required", true);
            }

            if (field.MinLength.HasValue)
            {
                builder.AddAttribute(sequence++, "minlength", field.MinLength.Value);
            }

            if (field.MaxLength.HasValue)
            {
                builder.AddAttribute(sequence++, "maxlength", field.MaxLength.Value);
            }

            if (!string.IsNullOrWhiteSpace(field.RangeMinimum))
            {
                builder.AddAttribute(sequence++, "min", field.RangeMinimum);
            }

            if (!string.IsNullOrWhiteSpace(field.RangeMaximum))
            {
                builder.AddAttribute(sequence++, "max", field.RangeMaximum);
            }

            if (GetFieldKind(field) == VxFormFieldKind.Checkbox)
            {
                builder.AddAttribute(sequence++, "checked", GetValue<bool>(field));
                builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.CreateBinder<bool>(this, value => SetValue(field, value), GetValue<bool>(field)));
            }
            else
            {
                var value = VxFormValueConverter.FormatValue(Model.Values[field.Name], field.FieldType);
                builder.AddAttribute(sequence++, "value", value);
                builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => SetValue(field, ConvertValue(field, value)), value));
            }

            builder.CloseElement();
        }

        private string GetFieldClass(VxFormFieldMetadata field)
        {
            return field.ColSpan.HasValue ? $"mb-3 col-{field.ColSpan.Value}" : "mb-3";
        }

        private static string GetInputType(VxFormFieldMetadata field)
        {
            var fieldKind = GetFieldKind(field);

            if (fieldKind == VxFormFieldKind.Checkbox)
            {
                return "checkbox";
            }

            if (VxFormValueConverter.GetValueType(field.FieldType) == typeof(DateTime))
            {
                return "date";
            }

            if (fieldKind == VxFormFieldKind.Number)
            {
                return "number";
            }

            return "text";
        }

        private static VxFormFieldKind GetFieldKind(VxFormFieldMetadata field)
        {
            if (field.FieldKind != VxFormFieldKind.Auto)
            {
                return field.FieldKind;
            }

            if (field.Options.Count > 0)
            {
                return VxFormFieldKind.Select;
            }

            if (VxFormValueConverter.GetValueType(field.FieldType) == typeof(bool))
            {
                return VxFormFieldKind.Checkbox;
            }

            var valueType = VxFormValueConverter.GetValueType(field.FieldType);
            if (valueType == typeof(decimal) || valueType == typeof(double) || valueType == typeof(float) || valueType == typeof(int) || valueType == typeof(long))
            {
                return VxFormFieldKind.Number;
            }

            return VxFormFieldKind.Text;
        }

        private T GetValue<T>(VxFormFieldMetadata field)
        {
            return Model.Values.TryGetValue(field.Name, out var value) && value is T typedValue ? typedValue : default;
        }

        private void SetValue(VxFormFieldMetadata field, object value)
        {
            Model.Values[field.Name] = value;
        }

        private bool IsVisible(VxFormFieldMetadata field)
        {
            return field.VisibilityRule == null || field.VisibilityRule.IsVisible(Model);
        }

        private static object ConvertValue(VxFormFieldMetadata field, string value)
        {
            return VxFormValueConverter.ConvertValue(field.FieldType, value);
        }
    }
}
