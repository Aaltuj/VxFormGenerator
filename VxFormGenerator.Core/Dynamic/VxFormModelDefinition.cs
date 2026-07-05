using System.Collections.Generic;

namespace VxFormGenerator.Core.Dynamic
{
    public sealed class VxFormModelDefinition
    {
        public string Namespace { get; set; } = "VxFormGenerator.Generated";

        public string ClassName { get; set; }

        public IList<string> Usings { get; } = new List<string>
        {
            "System",
            "System.ComponentModel.DataAnnotations",
            "VxFormGenerator.Core.Layout"
        };

        public IList<string> Attributes { get; } = new List<string>();

        public IList<VxFormModelPropertyDefinition> Properties { get; } = new List<VxFormModelPropertyDefinition>();
    }
}
