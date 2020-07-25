using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VxFormGenerator.Components
{
    public class BootstrapInputSelectWithOptions<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptions()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }

    }
}
