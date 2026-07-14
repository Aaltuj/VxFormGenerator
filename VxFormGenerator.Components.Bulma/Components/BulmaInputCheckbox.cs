using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bulma
{
    public class BulmaInputCheckbox : VxInputCheckbox
    {
        public BulmaInputCheckbox()
        {
            ContainerCss = "control";
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "checkbox" } };
            LabelCss = "checkbox";
        }
    }
}
