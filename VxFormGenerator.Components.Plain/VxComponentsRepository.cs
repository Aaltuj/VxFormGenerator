using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;
using VxFormGenerator.Core;
using VxFormGenerator.Models;
using VxFormGenerator.Core.Repository;
using VxFormGenerator.Core.Render;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Repository.Plain
{
    public class VxComponentsRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxComponentsRepository()
        {
            Setup(new[] { System.Reflection.Assembly.GetAssembly(typeof(VxComponentsRepository)) });
        }

        public VxComponentsRepository(System.Reflection.Assembly[] assemblies)
        {
            Setup(assemblies);
        }

        private void Setup(System.Reflection.Assembly[] assemblies)
        {

            var registrationDict = new Dictionary<Type, Type>()
                  {
                        {typeof(string), typeof(InputText) },
                        {typeof(DateTime), typeof(InputDate<>) },
                        {typeof(int), typeof(InputNumber<>) },
                       // {typeof(bool), typeof(VxInputCheckbox) },
                        {typeof(Enum), typeof(InputSelectWithOptions<>) },
                        {typeof(VxLookupKeyValue), typeof(InputSelectWithOptions<>) },
                        {typeof(ValueReferences), typeof(InputCheckboxMultiple<>) },
                        {typeof(decimal), typeof(InputNumber<>) },
                        {typeof(VxColor), typeof(InputColor) }
                  };


            _ComponentDict = VxDataTypeComponentRegistration.CreateRegistrationList(registrationDict);

            _DefaultComponent = null;

            this.RegisterAllDiscoverableFormElements(assemblies,
                (item, componentType) => new VxDataTypeComponentRegistration(item.SupportedDataType, componentType, item.IsSupported));
        }
    }
}
