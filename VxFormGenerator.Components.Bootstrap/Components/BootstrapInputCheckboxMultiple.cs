using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using VxFormGenerator.Core;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bootstrap
{
    public class BootstrapInputCheckboxMultiple<TValue> : InputCheckboxMultipleWithChildren<TValue>
    {
     
        protected static new Type TypeOfChildToRender { get => typeof(BootstrapInputCheckbox); }

        public BootstrapInputCheckboxMultiple()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }

    }
}
