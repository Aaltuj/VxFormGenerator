using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bootstrap
{
    public class BootstrapInputSelectWithOptionsComponent<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptionsComponent()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "custom-select" } };
        }

       
    }
}
