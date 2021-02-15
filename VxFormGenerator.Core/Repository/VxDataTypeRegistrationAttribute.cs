using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Repository
{
    public interface IVxComponentRegistration<T>
    {
        public T SupportedDataType { get; set; }
        public Func<object, bool>? IsSupported { get; set; }
    }
    public class VxDataTypeRegistrationAttribute : Attribute, IVxComponentRegistration<Type>
    {
        public Type SupportedDataType { get; set; }
        public Func<object, bool> IsSupported { get; set; }

    }

    public class VxDataTypeComponentRegistration : IVxComponentRegistration<Type>
    {
        public Type SupportedDataType { get; set; }
        public Func<object, bool> IsSupported { get; set; }

        public Type Component { get; set; }
    }
}
