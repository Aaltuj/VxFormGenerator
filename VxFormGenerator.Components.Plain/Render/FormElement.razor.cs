
using System.Collections.Generic;
using VxFormGenerator.Core;

namespace VxFormGenerator.Form
{
    public class FormElementComponent<TFormElement> : FormElementBase<TFormElement>
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();


            DefaultFieldClasses = new List<string>() { "form-control-plain" };
            CssClasses = new List<string>() { "form-group-plain" };
        }
    }
}