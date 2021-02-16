using System.Collections.Generic;
using VxFormGenerator.Core.Repository.Registration;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bootstrap
{
    [VxDataTypeRegistration(SupportedDataType = typeof(bool))]
    public class BootstrapInputCheckbox : VxInputCheckbox
    {
        public BootstrapInputCheckbox()
        {
            ContainerCss = "custom-control custom-checkbox line-height-checkbox";
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "custom-control-input" } };
            LabelCss = "custom-control-label";
        }
    }

}
