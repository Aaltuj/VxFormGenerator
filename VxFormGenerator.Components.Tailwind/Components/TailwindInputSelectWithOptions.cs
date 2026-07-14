using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Tailwind
{
    public class TailwindInputSelectWithOptions<TValue> : InputSelectWithOptions<TValue>
    {
        public TailwindInputSelectWithOptions()
        {
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500/30" } };
        }
    }
}
