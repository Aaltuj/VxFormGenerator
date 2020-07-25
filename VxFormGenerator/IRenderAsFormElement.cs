using System.Collections.Generic;
using System.Reflection;

namespace VxFormGenerator
{
    internal interface IRenderAsFormElement
    {
        public string Label { get; set; }
        public PropertyInfo FieldIdentifier { get; set; }

        public List<string> FormElementClasses { get; set; }
    }
}