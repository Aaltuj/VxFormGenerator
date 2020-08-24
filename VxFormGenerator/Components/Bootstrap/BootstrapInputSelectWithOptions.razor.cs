using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VxFormGenerator.Components.Plain;

namespace VxFormGenerator.Components.Bootstrap
{
    public class BootstrapInputSelectWithOptionsComponent<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptionsComponent()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "custom-select" } };
        }

       
    }
}
