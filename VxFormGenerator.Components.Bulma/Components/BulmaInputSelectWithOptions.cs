using System.Collections.Generic;
using VxFormGenerator.Form.Components.Plain;

namespace VxFormGenerator.Form.Components.Bulma
{
    public class BulmaInputSelectWithOptions<TValue> : InputSelectWithOptions<TValue>
    {
        public BulmaInputSelectWithOptions()
        {
            AdditionalAttributes = new Dictionary<string, object>() { { "class", "select is-fullwidth" } };
        }
    }
}
