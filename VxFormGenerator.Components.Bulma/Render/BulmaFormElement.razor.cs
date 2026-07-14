using System.Collections.Generic;
using VxFormGenerator.Core;

namespace VxFormGenerator.Render.Bulma
{
    public class BulmaFormElementComponent<TFormElement> : FormElementBase<TFormElement>
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            DefaultFieldClasses = new List<string>() { "input" };
            CssClasses = new List<string>() { "field" };
        }
    }
}
