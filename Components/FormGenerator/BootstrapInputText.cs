using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class BootstrapInputText : InputText
    {
        public BootstrapInputText()
        {
            this.AdditionalAttributes = new Dictionary<string, object>() { { "class", "form-control" } };
        }
    }

}
