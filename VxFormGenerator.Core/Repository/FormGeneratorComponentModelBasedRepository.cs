using System;
using System.Collections.Generic;
using System.Text;

namespace VxFormGenerator.Core.Repository
{
    public class FormGeneratorComponentModelBasedRepository : FormGeneratorComponentsRepository<Type>
    {
        protected override Type GetComponent(Type key)
        {
            var type = key;
            // When the type is an ENUM use Enum as type instead of property
            if (key.IsEnum)
            {
                type = typeof(Enum);
            }
            // When the type is a ValuesReferences use the base type. example ValuesReferences<bool>
            else if (key.BaseType == typeof(ValueReferences))
            {
                type = typeof(ValueReferences);
            }
            // When it's a Nullable type use the underlying type for matching
            else if(Nullable.GetUnderlyingType(key) != null )
            {
                type = Nullable.GetUnderlyingType(key);
            }

            return base.GetComponent(type);
        }
    }
}
