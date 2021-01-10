using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Render
{
    public class VxLookupResult<T>
    {
        public string Name { get; set; }

        public IDictionary<string, T> Values { get; set; } = new Dictionary<string, T>();

    }
}
