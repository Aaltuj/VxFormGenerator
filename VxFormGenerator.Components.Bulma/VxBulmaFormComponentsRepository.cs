using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Repository;
using VxFormGenerator.Form.Components.Bulma;
using VxFormGenerator.Form.Components.Plain;
using VxFormGenerator.Models;

namespace VxFormGenerator.Repository.Bulma
{
    public class VxBulmaRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxBulmaRepository()
        {
            _ComponentDict = new Dictionary<Type, Type>()
                  {
                    { typeof(string),          typeof(InputText) },
                    { typeof(DateTime),        typeof(InputDate<>) },
                    { typeof(bool),            typeof(BulmaInputCheckbox) },
                    { typeof(Enum),            typeof(BulmaInputSelectWithOptions<>) },
                    { typeof(ValueReferences), typeof(BulmaInputCheckboxMultiple<>) },
                    { typeof(decimal),         typeof(InputNumber<>) },
                    { typeof(System.Single),   typeof(InputNumber<>) },
                    { typeof(int),             typeof(InputNumber<>) },
                    { typeof(VxColor),         typeof(InputColor) }
                  };

            _DefaultComponent = null;
        }
    }
}
