using System.Collections.Generic;
using VxFormGenerator.Core;

namespace VxFormGenerator.Render.Bootstrap
{

    public class BootstrapFormElementComponent<TFormElement> : FormElementBase<TFormElement>
    {
        public BootstrapFormElementComponent()
        {

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();


        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();


            DefaultFieldClasses = new List<string>() { "form-control" };
            CssClasses = new List<string>() { "form-group" };
        }
    }
}
