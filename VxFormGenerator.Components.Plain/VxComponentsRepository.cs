using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;
using VxFormGenerator.Core;
using VxFormGenerator.Models;
using VxFormGenerator.Core.Repository;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Repository.Plain
{
    public class VxComponentsRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxComponentsRepository()
        {

            _ComponentDict = new Dictionary<Type, Type>()
                  {
                        {typeof(string), typeof(InputText) },
                        {typeof(DateTime), typeof(InputDate<>) },
                        {typeof(int), typeof(InputNumber<>) },
                        {typeof(bool), typeof(VxInputCheckbox) },
                        {typeof(Enum), typeof(InputSelectWithOptions<>) },
                        {typeof(VxLookupKeyValue), typeof(InputSelectWithOptions<>) },
                        {typeof(ValueReferences), typeof(InputCheckboxMultiple<>) },
                        {typeof(decimal), typeof(InputNumber<>) },
                        {typeof(VxColor), typeof(InputColor) }
                  };
            _DefaultComponent = null;
         
            
        }
     
    }
}
