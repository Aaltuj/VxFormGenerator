using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class BootstrapInputCheckbox : InputCheckbox, IRenderCss
    {
        private List<string> cssClasses;

        public List<string> CssClasses { get => cssClasses; set => cssClasses = value; }
        public List<string> FormElementCssClasses { get; set; }

        public BootstrapInputCheckbox()
        {
            CssClasses = new List<string>() { "form-check-input" };
        }

    }

}
