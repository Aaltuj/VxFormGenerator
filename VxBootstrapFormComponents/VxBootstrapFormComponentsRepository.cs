using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxBootstrapFormComponents.Components;
using VxFormGenerator;
using VxFormComponents.Components;
using VxFormGenerator.Models;
using VxFormGenerator.Repository;

namespace VxBootstrapFormComponents
{
    public class VxBootstrapFormComponentsRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxBootstrapFormComponentsRepository()
        {

            _ComponentDict = new Dictionary<Type, Type>()
                  {
                    { typeof(string),          typeof(BootstrapInputText) },
                    { typeof(DateTime),        typeof(InputDate<>) },
                    { typeof(bool),            typeof(BootstrapInputCheckbox) },
                    { typeof(Enum),            typeof(BootstrapInputSelectWithOptions<>) },
                    { typeof(ValueReferences), typeof(BootstrapInputCheckboxMultiple<>) },
                    { typeof(decimal),         typeof(BootstrapInputNumber<>) },
                    { typeof(VxColor),         typeof(InputColor) }
                  };

            _DefaultComponent = null;


        }

    }
}
