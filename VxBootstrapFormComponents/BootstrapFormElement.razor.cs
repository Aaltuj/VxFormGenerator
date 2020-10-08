using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VxFormGenerator;

namespace VxBootstrapFormComponents
{

    public class BootstrapFormElementComponent<TFormElement> : FormElementBase<TFormElement>
    {
        public BootstrapFormElementComponent()
        {
            DefaultFieldClasses = new List<string>() { "form-control"};
            CssClasses = new List<string>() { "form-group", "row" };
        }
    }
}
