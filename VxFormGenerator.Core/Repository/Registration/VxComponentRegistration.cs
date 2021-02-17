using System;
using System.Collections.Generic;
using System.Linq;

namespace VxFormGenerator.Core.Repository.Registration
{
    public interface IVxComponentRegistration<T> : IVxComponentAttributeRegistration<T>
    {
        public Type Component { get; set; }

    }

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
        public VxDataTypeComponentRegistration(Type supportedDataType, Type componentType, Func<object, bool> IsSupported = null)
        {
            Component = componentType;
            SupportedDataType = supportedDataType;
            this.IsSupported = IsSupported;
        }

        public static Dictionary<Type, IList<IVxComponentRegistration<Type>>> CreateRegistrationList(IDictionary<Type, Type> registrations)
        {
            var componentRegistrations = registrations
                .Select(item => new VxDataTypeComponentRegistration(item.Key, item.Value));

            var result = new Dictionary<Type, IList<IVxComponentRegistration<Type>>>();

            foreach (var registration in componentRegistrations)
                result.Add(registration.SupportedDataType, new List<IVxComponentRegistration<Type>>()
            {registration });

            return result;
        }
    }
}
