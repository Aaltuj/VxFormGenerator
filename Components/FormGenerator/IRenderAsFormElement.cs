using System.Collections.Generic;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    internal interface IRenderAsFormElement
    {
        public string Label { get; set; }
        public PropertyInfo FieldIdentifier { get; set; }

        public List<string> FormElementClasses { get; set; }
    }
}