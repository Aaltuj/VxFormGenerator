using Microsoft.AspNetCore.Components;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Core
{
    public sealed class VxFormFieldTemplateContext
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string Label { get; set; }

        public bool ShowLabel { get; set; }

        public VxFormElementDefinition FieldDefinition { get; set; }

        public RenderFragment Input { get; set; }

        public RenderFragment ValidationMessage { get; set; }
    }
}
