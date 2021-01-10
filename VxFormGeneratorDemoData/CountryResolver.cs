using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VxFormGenerator.Core.Render;

namespace VxFormGeneratorDemoData
{
    public class CountryResolver : VxLookupKeyValue
    {
        public override string Name { get; set; } = "COUNTRY_LOOKUP";

        public override Task<VxLookupResult<string>> GetLookupValues(object param)
        {
            return Task.FromResult(new VxLookupResult<string>() { Name = Name, Values = new Dictionary<string, string>() { { "NL", "Netherlands" } } });
        }
    }
}