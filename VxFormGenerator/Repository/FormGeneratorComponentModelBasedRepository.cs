using System;
using System.Collections.Generic;
using System.Text;

namespace VxFormGenerator.Repository
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
            else if (key.BaseType == typeof(ValueReferences))
            {
                type = typeof(ValueReferences);
            }

            return base.GetComponent(type);
        }
    }
}
