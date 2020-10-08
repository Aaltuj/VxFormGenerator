using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VxFormComponents.Components;

namespace VxBootstrapFormComponents.Components
{
    public class BootstrapInputSelectWithOptionsComponent<TValue>: InputSelectWithOptions<TValue>
    {
        public BootstrapInputSelectWithOptionsComponent()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "custom-select" } };
        }

       
    }
}
