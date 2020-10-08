using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormComponents.Components;
using VxFormGenerator;
using VxFormGenerator.Models;
using VxFormGenerator.Repository;

namespace VxBootstrapFormComponents
{
    public class VxComponentsRepository : FormGeneratorComponentModelBasedRepository
    {
        public VxComponentsRepository()
        {

            _ComponentDict = new Dictionary<Type, Type>()
                  {
                        {typeof(string), typeof(VxInputText) },
                        {typeof(DateTime), typeof(InputDate<>) },
                        {typeof(bool), typeof(VxInputCheckbox) },
                        {typeof(Enum), typeof(InputSelectWithOptions<>) },
                        {typeof(ValueReferences), typeof(InputCheckboxMultiple<>) },
                        {typeof(decimal), typeof(InputNumber<>) },
                        {typeof(VxColor), typeof(InputColor) }
                  };
            _DefaultComponent = null;
         
            
        }
     
    }
}
