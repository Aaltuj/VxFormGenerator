using System;
using System.Collections.Generic;
using System.Text;

namespace VxFormGenerator.Core.Layout
{
    public class VxFormRow<TModel>
    {

        public string Label { get; set; }

        public string Id { get; set; }

        public LabelPositions LabelPosition { get; set; }

        public Array<VxFormColumn> { get; set; }

}
}
