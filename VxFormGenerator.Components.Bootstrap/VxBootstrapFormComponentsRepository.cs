using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormGenerator.Form.Components.Bootstrap;
using VxFormGenerator.Models;
using VxFormGenerator.Core.Repository;
using VxFormGenerator.Core;
using VxFormGenerator.Form.Components.Plain;
using VxFormGenerator.Core.Render;
using System.Reflection;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Repository.Bootstrap
{
    public class VxBootstrapRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxBootstrapRepository()
        {
            Setup(new[] { System.Reflection.Assembly.GetAssembly(typeof(VxBootstrapRepository)) });
        }

        public VxBootstrapRepository(System.Reflection.Assembly[] assemblies)
        {
            Setup(assemblies);
        }

        private void Setup(System.Reflection.Assembly[] assemblies)
        {
            var registrationDict = new Dictionary<Type, Type>()
                  {
                    { typeof(string),          typeof(InputText) },
                    { typeof(DateTime),        typeof(InputDate<>) },
                    { typeof(bool),            typeof(BootstrapInputCheckbox) },
                    { typeof(Enum),            typeof(BootstrapInputSelectWithOptions<>) },
                    { typeof(IDictionary<bool, Enum>), typeof(BootstrapInputCheckboxMultiple<>) },
                    { typeof(VxLookupKeyValue),typeof(BootstrapInputSelectWithOptions<>) },
                    { typeof(decimal),         typeof(InputNumber<>) },
                    { typeof(int),             typeof(InputNumber<>) },
                    { typeof(VxColor),         typeof(InputColor) }
                  };


            _ComponentDict = VxDataTypeComponentRegistration.CreateRegistrationList(registrationDict);

            _DefaultComponent = null;

            this.RegisterAllDiscoverableFormElements(assemblies,
                (item, componentType) => new VxDataTypeComponentRegistration(item.SupportedDataType, componentType, item.IsSupported));
        }
    }
}
