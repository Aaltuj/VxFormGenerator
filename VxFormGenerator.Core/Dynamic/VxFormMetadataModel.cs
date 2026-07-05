using System.Collections.Generic;

namespace VxFormGenerator.Core.Dynamic
{
    public sealed class VxFormMetadataModel
    {
        public IList<VxFormFieldMetadata> Fields { get; } = new List<VxFormFieldMetadata>();

        public IDictionary<string, object> Values { get; } = new Dictionary<string, object>();
    }
}
