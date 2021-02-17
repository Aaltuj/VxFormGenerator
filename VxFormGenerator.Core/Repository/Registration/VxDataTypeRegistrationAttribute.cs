using System;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Repository.Registration
{
    public interface IVxComponentAttributeRegistration<T>
    {
        public T SupportedDataType { get; set; }
        public Func<object, bool>? IsSupported { get; set; }

    }

 

    public class VxDataTypeRegistrationAttribute : Attribute, IVxComponentAttributeRegistration<Type>
    {
        public Type SupportedDataType { get; set; }
        public Func<object, bool> IsSupported { get; set; }

    }
}
