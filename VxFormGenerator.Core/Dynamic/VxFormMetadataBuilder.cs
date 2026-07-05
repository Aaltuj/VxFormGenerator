using System;
using System.Text;

namespace VxFormGenerator.Core.Dynamic
{
    public static class VxFormMetadataBuilder
    {
        public static VxFormMetadataModel Build(VxFormModelDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            var model = new VxFormMetadataModel();

            foreach (var property in definition.Properties)
            {
                var fieldType = VxFormRuntimeModelBuilder.ResolvePropertyType(property);
                var field = new VxFormFieldMetadata
                {
                    Name = property.Name,
                    Id = string.IsNullOrWhiteSpace(property.Id) ? CreateFieldId(property.Name) : property.Id,
                    FieldType = fieldType,
                    FieldKind = property.FieldKind,
                    Label = property.Label,
                    Placeholder = property.Placeholder,
                    Description = property.Description,
                    RowId = property.RowId,
                    RowLabel = property.RowLabel,
                    ColSpan = property.ColSpan,
                    Order = property.Order,
                    IsRequired = property.IsRequired,
                    MinLength = property.MinLength,
                    MaxLength = property.MaxLength,
                    RangeMinimum = property.RangeMinimum,
                    RangeMaximum = property.RangeMaximum
                };

                foreach (var option in property.Options)
                {
                    field.Options.Add(option);
                }

                model.Fields.Add(field);
                model.Values[field.Name] = GetDefaultValue(field, fieldType);
            }

            return model;
        }

        private static object GetDefaultValue(VxFormFieldMetadata field, Type type)
        {
            foreach (var option in field.Options)
            {
                if (option.IsSelected)
                {
                    return VxFormValueConverter.ConvertValue(type, option.Value);
                }
            }

            return VxFormValueConverter.GetDefaultValue(type);
        }

        private static string CreateFieldId(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "vx-field";
            }

            var id = new StringBuilder("vx-");

            foreach (var character in name.Trim())
            {
                id.Append(char.IsLetterOrDigit(character) || character == '_' || character == '-' ? char.ToLowerInvariant(character) : '-');
            }

            return id.ToString();
        }
    }
}
