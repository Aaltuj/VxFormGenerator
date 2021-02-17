using System;

namespace VxFormGenerator.Core.Repository.Registration
{
    public class VxStringBasedRegistrationAttribute : Attribute, IVxComponentAttributeRegistration<string>
    {
        public string SupportedDataType { get; set; }
        public Func<object, bool> IsSupported { get; set; }

    }
}
