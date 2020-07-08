using System.Collections.Generic;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public interface IRenderCss
    {
        public List<string> CssClasses { get; set; }

        public List<string> FormElementCssClasses { get; set; }
    }
}