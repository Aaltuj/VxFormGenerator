using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Core.Dynamic
{
    public static class VxFormRuntimeModelBuilder
    {
        private static readonly Dictionary<string, Type> TypeAliases = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            ["bool"] = typeof(bool),
            ["boolean"] = typeof(bool),
            ["byte"] = typeof(byte),
            ["datetime"] = typeof(DateTime),
            ["decimal"] = typeof(decimal),
            ["double"] = typeof(double),
            ["float"] = typeof(float),
            ["int"] = typeof(int),
            ["int32"] = typeof(int),
            ["long"] = typeof(long),
            ["bool?"] = typeof(bool?),
            ["boolean?"] = typeof(bool?),
            ["datetime?"] = typeof(DateTime?),
            ["decimal?"] = typeof(decimal?),
            ["double?"] = typeof(double?),
            ["float?"] = typeof(float?),
            ["int?"] = typeof(int?),
            ["int32?"] = typeof(int?),
            ["long?"] = typeof(long?),
            ["string"] = typeof(string)
        };

        public static Type BuildType(VxFormModelDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (string.IsNullOrWhiteSpace(definition.ClassName))
            {
                throw new ArgumentException("A class name is required.", nameof(definition));
            }

            var typeName = string.IsNullOrWhiteSpace(definition.Namespace)
                ? ToIdentifier(definition.ClassName)
                : definition.Namespace + "." + ToIdentifier(definition.ClassName);

            var assemblyName = new AssemblyName("VxFormGenerator.DynamicModels." + Guid.NewGuid().ToString("N"));
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);

            foreach (var property in definition.Properties)
            {
                AddProperty(typeBuilder, property);
            }

            return typeBuilder.CreateTypeInfo().AsType();
        }

        public static object CreateInstance(VxFormModelDefinition definition)
        {
            return Activator.CreateInstance(BuildType(definition));
        }

        private static void AddProperty(TypeBuilder typeBuilder, VxFormModelPropertyDefinition property)
        {
            if (property == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(property.Name))
            {
                throw new ArgumentException("Every generated property requires a name.");
            }

                var propertyType = ResolvePropertyType(property);
            var propertyName = ToIdentifier(property.Name);
            var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            var getMethod = typeBuilder.DefineMethod(
                "get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);
            var getIl = getMethod.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setMethod = typeBuilder.DefineMethod(
                "set_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new[] { propertyType });
            var setIl = setMethod.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethod);
            propertyBuilder.SetSetMethod(setMethod);

            AddAttributes(propertyBuilder, property, propertyType);
        }

        private static void AddAttributes(PropertyBuilder propertyBuilder, VxFormModelPropertyDefinition property, Type propertyType)
        {
            if (!string.IsNullOrWhiteSpace(property.Label) || property.Order.HasValue)
            {
                var displayBuilder = new CustomAttributeBuilder(
                    typeof(DisplayAttribute).GetConstructor(Type.EmptyTypes),
                    Array.Empty<object>(),
                    GetDisplayAttributeProperties(property),
                    GetDisplayAttributeValues(property));

                propertyBuilder.SetCustomAttribute(displayBuilder);
            }

            var layoutProperties = GetLayoutAttributeProperties(property);
            if (layoutProperties.Length > 0)
            {
                var layoutBuilder = new CustomAttributeBuilder(
                    typeof(VxFormElementLayoutAttribute).GetConstructor(Type.EmptyTypes),
                    Array.Empty<object>(),
                    layoutProperties,
                    GetLayoutAttributeValues(property));

                propertyBuilder.SetCustomAttribute(layoutBuilder);
            }

            if (property.IsRequired)
            {
                propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(RequiredAttribute).GetConstructor(Type.EmptyTypes), Array.Empty<object>()));
            }

            if (property.MaxLength.HasValue)
            {
                var constructor = typeof(StringLengthAttribute).GetConstructor(new[] { typeof(int) });
                if (property.MinLength.HasValue)
                {
                    propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                        constructor,
                        new object[] { property.MaxLength.Value },
                        new[] { typeof(StringLengthAttribute).GetProperty(nameof(StringLengthAttribute.MinimumLength)) },
                        new object[] { property.MinLength.Value }));
                }
                else
                {
                    propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { property.MaxLength.Value }));
                }
            }
            else if (property.MinLength.HasValue)
            {
                propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(MinLengthAttribute).GetConstructor(new[] { typeof(int) }),
                    new object[] { property.MinLength.Value }));
            }

            if (!string.IsNullOrWhiteSpace(property.RangeMinimum) && !string.IsNullOrWhiteSpace(property.RangeMaximum))
            {
                propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(RangeAttribute).GetConstructor(new[] { typeof(Type), typeof(string), typeof(string) }),
                    new object[] { propertyType, property.RangeMinimum, property.RangeMaximum }));
            }
        }

        private static PropertyInfo[] GetDisplayAttributeProperties(VxFormModelPropertyDefinition property)
        {
            var properties = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(property.Label))
            {
                properties.Add(typeof(DisplayAttribute).GetProperty(nameof(DisplayAttribute.Name)));
            }

            if (property.Order.HasValue)
            {
                properties.Add(typeof(DisplayAttribute).GetProperty(nameof(DisplayAttribute.Order)));
            }

            return properties.ToArray();
        }

        private static object[] GetDisplayAttributeValues(VxFormModelPropertyDefinition property)
        {
            var values = new List<object>();

            if (!string.IsNullOrWhiteSpace(property.Label))
            {
                values.Add(property.Label);
            }

            if (property.Order.HasValue)
            {
                values.Add(property.Order.Value);
            }

            return values.ToArray();
        }

        private static PropertyInfo[] GetLayoutAttributeProperties(VxFormModelPropertyDefinition property)
        {
            var properties = new List<PropertyInfo>();

            AddPropertyInfo(properties, property.RowId.HasValue, nameof(VxFormElementLayoutAttribute.RowId));
            AddPropertyInfo(properties, property.ColSpan.HasValue, nameof(VxFormElementLayoutAttribute.ColSpan));
            AddPropertyInfo(properties, !string.IsNullOrWhiteSpace(property.Label), nameof(VxFormElementLayoutAttribute.Label));
            AddPropertyInfo(properties, !string.IsNullOrWhiteSpace(property.Placeholder), nameof(VxFormElementLayoutAttribute.Placeholder));
            AddPropertyInfo(properties, !string.IsNullOrWhiteSpace(property.Description), nameof(VxFormElementLayoutAttribute.Description));
            AddPropertyInfo(properties, property.Order.HasValue, nameof(VxFormElementLayoutAttribute.Order));

            return properties.ToArray();
        }

        private static object[] GetLayoutAttributeValues(VxFormModelPropertyDefinition property)
        {
            var values = new List<object>();

            AddValue(values, property.RowId.HasValue, property.RowId.GetValueOrDefault());
            AddValue(values, property.ColSpan.HasValue, property.ColSpan.GetValueOrDefault());
            AddValue(values, !string.IsNullOrWhiteSpace(property.Label), property.Label);
            AddValue(values, !string.IsNullOrWhiteSpace(property.Placeholder), property.Placeholder);
            AddValue(values, !string.IsNullOrWhiteSpace(property.Description), property.Description);
            AddValue(values, property.Order.HasValue, property.Order.GetValueOrDefault());

            return values.ToArray();
        }

        private static void AddPropertyInfo(List<PropertyInfo> properties, bool condition, string propertyName)
        {
            if (condition)
            {
                properties.Add(typeof(VxFormElementLayoutAttribute).GetProperty(propertyName));
            }
        }

        private static void AddValue(List<object> values, bool condition, object value)
        {
            if (condition)
            {
                values.Add(value);
            }
        }

        public static Type ResolvePropertyType(VxFormModelPropertyDefinition property)
        {
            if (property.RuntimeType != null)
            {
                return property.RuntimeType;
            }

            if (!string.IsNullOrWhiteSpace(property.TypeName) && TypeAliases.TryGetValue(property.TypeName, out var aliasType))
            {
                return aliasType;
            }

            if (!string.IsNullOrWhiteSpace(property.TypeName))
            {
                var resolvedType = Type.GetType(property.TypeName, throwOnError: false);
                if (resolvedType != null)
                {
                    return resolvedType;
                }
            }

            return typeof(string);
        }

        private static string ToIdentifier(string value)
        {
            var identifier = value.Trim().Replace(' ', '_').Replace('-', '_');
            return char.IsDigit(identifier[0]) ? "_" + identifier : identifier;
        }
    }
}
