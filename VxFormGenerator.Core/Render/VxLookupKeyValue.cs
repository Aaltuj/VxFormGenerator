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

        public abstract Task<VxLookupResult<string>> GetLookupValues(object param = null);

    }

    public class VxEnumLookup : VxLookupKeyValue
    {
        public override string Name { get; set; } = "ENUM_LOOKUP";

        public Type Enum { get; set; }

        public override Task<VxLookupResult<string>> GetLookupValues(object param = null)
        {
            if (!Enum.IsEnum)
                throw new Exception("Expected an Enum type");

            var values = Enum.GetEnumValues();

            var dict = new Dictionary<string, string>();

            foreach (var item in values)
            {
                var selectItem = VxSelectItem.ToSelectItem(item as Enum);
                dict.Add(item.ToString(), selectItem.Label);
            }

            return Task.FromResult(new VxLookupResult<string>() { Name = Enum.GetType().Name, Values = dict });
        }
    }

}
