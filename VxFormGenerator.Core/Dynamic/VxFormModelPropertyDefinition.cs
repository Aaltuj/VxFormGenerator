using System.Collections.Generic;
using System;

namespace VxFormGenerator.Core.Dynamic
{
    public sealed class VxFormModelPropertyDefinition
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string TypeName { get; set; } = "string";

        public Type RuntimeType { get; set; }

        public VxFormFieldKind FieldKind { get; set; } = VxFormFieldKind.Auto;

        public string Label { get; set; }

        public string Placeholder { get; set; }

        public string Description { get; set; }

        public int? RowId { get; set; }

        public int? ColSpan { get; set; }

        public int? Order { get; set; }

        public bool IsRequired { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string RangeMinimum { get; set; }
        public string RangeMaximum { get; set; }

        public string DefaultValueExpression { get; set; }

        public IList<string> Attributes { get; } = new List<string>();

        public IList<VxFormLookupOption> Options { get; } = new List<VxFormLookupOption>();
    }
}
