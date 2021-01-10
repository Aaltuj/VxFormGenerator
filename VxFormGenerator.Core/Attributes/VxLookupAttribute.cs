using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Core.Attributes
{

    public class VxLookupResolverResult
    {
        public VxLookupKeyValue LookupKeyValue { get; private set; }
        public VxLookupResolverResult(object result)
        {
            if (result.GetType().BaseType.Equals(typeof(VxLookupKeyValue)))
            {
                LookupKeyValue = (VxLookupKeyValue)(object)result;
            }
        }


    }

    public class VxLookupAttribute : Attribute
    {
        private Type _Resolver { get; set; }

        public VxLookupAttribute(Type resolver)
        {
            // TODO: add type constrains vxlookup
            _Resolver = resolver;
        }

        public VxLookupResolverResult GetResolver()
        {
            var result = Activator.CreateInstance(_Resolver);
            return new VxLookupResolverResult(result);
        }
    }
}
