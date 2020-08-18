using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VxFormGenerator.Components.Plain;

namespace VxFormGenerator.Components.Bootstrap
{
    public class BootstrapInputSelectWithOptions<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptions()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }

    }
}
