using System;
using System.Collections.Generic;
using System.Linq;

namespace VxFormGenerator.Core.Repository.Registration
{
    public class VxDataTypeComponentRegistration : IVxComponentRegistration<Type>
    {
        public Type SupportedDataType { get; set; }
        public Func<object, bool>? IsSupported { get; set; }

        public Type Component { get; set; }

        public VxDataTypeComponentRegistration(Type supportedDataType, Type componentType)
        {
            Component = componentType;
            SupportedDataType = supportedDataType;
        }

        public static Dictionary<Type, IList<VxDataTypeComponentRegistration>> CreateRegistrationList(IDictionary<Type, Type> registrations)
        {
            var componentRegistrations = registrations
                .Select(item => new VxDataTypeComponentRegistration(item.Key, item.Value));

            var result = new Dictionary<Type, IList<VxDataTypeComponentRegistration>>();

            foreach (var registration in componentRegistrations)
                result.Add(registration.SupportedDataType, new List<VxDataTypeComponentRegistration>()
            {registration });

            return result;
        }
    }
}
