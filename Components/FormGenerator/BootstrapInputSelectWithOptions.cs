using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class BootstrapInputSelectWithOptions<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptions()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }

    }
}
