using System.Collections.Generic;
using VxFormGenerator.Components.Plain;

namespace VxFormGenerator.Components.Bootstrap
{
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
