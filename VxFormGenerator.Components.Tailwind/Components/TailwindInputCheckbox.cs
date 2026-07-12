using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Tailwind
{
    public class TailwindInputCheckbox : VxInputCheckbox
    {
        public TailwindInputCheckbox()
        {
            ContainerCss = "flex items-center gap-2";
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500" } };
            LabelCss = "text-sm font-medium text-gray-700";
        }
    }
}
