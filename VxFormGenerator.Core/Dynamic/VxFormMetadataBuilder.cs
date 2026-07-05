using System;

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
                    FieldType = fieldType,
                    Label = property.Label,
                    Placeholder = property.Placeholder,
                    Description = property.Description,
                    RowId = property.RowId,
                    ColSpan = property.ColSpan,
                    Order = property.Order,
                    IsRequired = property.IsRequired,
                    MinLength = property.MinLength,
                    MaxLength = property.MaxLength,
                    RangeMinimum = property.RangeMinimum,
                    RangeMaximum = property.RangeMaximum
                };

                model.Fields.Add(field);
                model.Values[field.Name] = GetDefaultValue(fieldType);
            }

            return model;
        }

        private static object GetDefaultValue(Type type)
        {
            if (type == typeof(string))
            {
                return string.Empty;
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
