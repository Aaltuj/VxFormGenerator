using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Render
{
    public abstract class VxLookupKeyValue : IVxLookupBase<string>
    {
        public abstract string Name { get; set; }

        public abstract Task<VxLookupResult<string>> GetLookupValues(object param);

    }
}
