using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class ValueReference<TKey, TValue>
    {
        public TValue Value { get; set; }
        public TKey Key { get; set; }

    }
}
