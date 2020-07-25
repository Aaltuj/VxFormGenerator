using System.Collections.Generic;

namespace VxFormGenerator
{
    public interface IRenderCss
    {
        public List<string> CssClasses { get; set; }

        public List<string> FormElementCssClasses { get; set; }
    }
}