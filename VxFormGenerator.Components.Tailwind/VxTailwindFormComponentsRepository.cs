using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Repository;
using VxFormGenerator.Form.Components.Plain;
using VxFormGenerator.Form.Components.Tailwind;
using VxFormGenerator.Models;

namespace VxFormGenerator.Repository.Tailwind
{
    public class VxTailwindRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxTailwindRepository()
        {
            _ComponentDict = new Dictionary<Type, Type>()
                  {
                    { typeof(string),          typeof(InputText) },
                    { typeof(DateTime),        typeof(InputDate<>) },
                    { typeof(bool),            typeof(TailwindInputCheckbox) },
                    { typeof(Enum),            typeof(TailwindInputSelectWithOptions<>) },
                    { typeof(ValueReferences), typeof(TailwindInputCheckboxMultiple<>) },
                    { typeof(decimal),         typeof(InputNumber<>) },
                    { typeof(System.Single),   typeof(InputNumber<>) },
                    { typeof(int),             typeof(InputNumber<>) },
                    { typeof(VxColor),         typeof(InputColor) }
                  };

            _DefaultComponent = null;
        }
    }
}
