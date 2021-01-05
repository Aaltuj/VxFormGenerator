using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Layout
{

    public class VxFormElementLayoutAttribute : Attribute
    {

        public int ColSpan { get; set; }

        public int RowId { get; set; }

        public string Label { get; set; }

        public string Placeholder { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
        public string FieldIdentifier { get; }
        public bool ShowLabel { get; internal set; } = true;
    }

    public class VxFormGroupAttribute : Attribute
    {
        public string Label { get; set; }
        public int Order { get; set; }
    }

    public class VxFormRowLayoutAttribute : Attribute
    {
        public string Label { get; set; }
        public int Order { get; set; }
        public int Id { get; set; }
    }
}
