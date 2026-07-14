using System.Collections.Generic;
using VxFormGenerator.Core;

namespace VxFormGenerator.Render.Tailwind
{
    public class TailwindFormElementComponent<TFormElement> : FormElementBase<TFormElement>
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            DefaultFieldClasses = new List<string>() { "w-full", "rounded-md", "border", "border-gray-300", "px-3", "py-2", "text-sm", "shadow-sm", "focus:border-blue-500", "focus:outline-none", "focus:ring-2", "focus:ring-blue-500/30" };
            CssClasses = new List<string>() { "mb-4" };
        }
    }
}
