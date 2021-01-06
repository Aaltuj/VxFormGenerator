using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bootstrap
{
    public class BootstrapInputSelectWithOptions<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptions()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "custom-select" } };
        }

       
    }
}
