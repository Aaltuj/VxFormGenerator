using System;
using System.Collections.Generic;
using System.Text;

namespace VxFormGenerator.Core.Layout
{

    public class VxFormGroup<TModel>
    {

        public string Label { get; set; }

        public string Id { get; set; }

        public Array<VxFormRow<TModel>> { get; set; }

}
}
