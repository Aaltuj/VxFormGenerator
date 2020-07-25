using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VxFormGenerator
{
    public class ValueReference<TKey, TValue>
    {
        public TValue Value { get; set; }
        public TKey Key { get; set; }

    }

    public class ValueReferences<T> : List<ValueReference<string, bool>>
    {
        public ValueReferences()
        {
            var values = typeof(T).GetEnumValues()
                .Cast<T>()
                .Select(m => new ValueReference<string, bool>() { Key = m.ToString(), Value = false })
                .ToList();

            this.AddRange(values);

        }
    }
}
