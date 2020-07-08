using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormGeneratorDemo.Components.FormGenerator
{

    public class BootstrapFormElementComponent : FormElementComponent
    {
        public BootstrapFormElementComponent()
        {
            DefaultFieldClasses = new List<string>() { "form-control"};
            CssClasses = new List<string>() { "form-group", "row" };
        }
    }
}
