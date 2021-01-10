using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Render
{
    public interface IVxLookupBase<T>
    {
        public abstract string Name { get; set; }

        public abstract Task<VxLookupResult<T>> GetLookupValues(object param);
    }
}
