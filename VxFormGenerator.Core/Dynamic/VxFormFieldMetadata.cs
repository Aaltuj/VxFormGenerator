using System;

namespace VxFormGenerator.Core.Dynamic
{
    public sealed class VxFormFieldMetadata
    {
        public string Name { get; set; }
        public Type FieldType { get; set; } = typeof(string);
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
    }
}
