using System;
using System.Collections.Generic;
using System.Linq;

namespace VxFormGenerator.Core.Repository.Registration
{
    public class VxStringBasedComponentRegistration : IVxComponentAttributeRegistration<string>
    {
        public string SupportedDataType { get; set; }
        public Func<object, bool>? IsSupported { get; set; }

        public Type Component { get; set; }

        public VxStringBasedComponentRegistration(string supportedDataType, Type componentType)
        {
            Component = componentType;
            SupportedDataType = supportedDataType;
        }

        public static Dictionary<string, IList<VxStringBasedComponentRegistration>> CreateRegistrationList(IDictionary<string, Type> registrations)
        {
            var componentRegistrations = registrations
                  .Select(item => new VxStringBasedComponentRegistration(item.Key, item.Value));

            var result = new Dictionary<string, IList<VxStringBasedComponentRegistration>>();

            foreach (var registration in componentRegistrations)
                result.Add(registration.SupportedDataType, new List<VxStringBasedComponentRegistration>()
            {registration });

            return result;
        }
    }
}
