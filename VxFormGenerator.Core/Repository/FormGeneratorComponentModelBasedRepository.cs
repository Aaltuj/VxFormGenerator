using System;
using System.Collections.Generic;
using VxFormGenerator.Core.Attributes;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Core.Repository
{
    public class FormGeneratorComponentModelBasedRepository : FormGeneratorComponentsRepository<Type, VxDataTypeRegistrationAttribute>
    {
        protected override Type GetComponent(Type key, Layout.VxFormElementDefinition formElementDefinition)
        {
            var type = key;
            if (formElementDefinition.HasLookup)
            {
                type = formElementDefinition.GetLookup.LookupKeyValue != null ? formElementDefinition.GetLookup.LookupKeyValue.GetType().BaseType : null;
            }
            // When the type is an ENUM use Enum as type instead of property
            else if (key.IsEnum)
            {
                type = typeof(Enum);
            }
            // When the type is a ValuesReferences use the base type. example ValuesReferences<bool>
            else if (key.BaseType == typeof(ValueReferences))
            {
                type = typeof(ValueReferences);
            }
            // the type is an dictionary
            else if (key.IsGenericType && key.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type keyType = key.GetGenericArguments()[0];
                Type valueType = key.GetGenericArguments()[1];

                if (keyType.IsEnum && valueType == typeof(bool))
                    type = typeof(IDictionary<Enum, bool>);
                if (keyType.IsSubclassOf(typeof(string)) && valueType == typeof(bool))
                    type = typeof(IDictionary<string, bool>);
            }
            // When it's a Nullable type use the underlying type for matching
            else if (Nullable.GetUnderlyingType(key) != null)
            {
                type = Nullable.GetUnderlyingType(key);
            }

            return base.GetComponent(type, formElementDefinition);
        }


    }
}
