using Bunit;
using System;
using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Attributes;
using VxFormGenerator.Core.Layout;
using VxFormGeneratorDemoData;
using Xunit;
using System.Reflection;

namespace VxFormGenerator.Core.Tests
{
    public class VxHelpers
    {
        [Fact]
        public void CreateVxColumn()
        {
            var definition = VxFormDefinition.CreateFromModel(typeof(AddressViewModel), new VxFormLayoutOptions());

        }

        [Fact]
        public void EnumExtentionGetAttribute()
        {

            var value = VisualFeedbackValidationPolicyAnnotated.VALID_AND_INVALID;
            var attribute = value.GetAttribute<DisplayAttribute>();
            Assert.Equal("When valid or invalid", attribute.Name);

        }

        [Fact]
        public async System.Threading.Tasks.Task GetLookupAsync()
        {
            var p = typeof(AddressViewModel).GetProperty("Country");

            var attribute = p.GetCustomAttribute(typeof(VxLookupAttribute), true) as VxLookupAttribute;
            Assert.NotNull(attribute);

            var resolver = attribute.GetResolver().LookupKeyValue;
            Assert.Equal("COUNTRY_LOOKUP", resolver.Name);
            var result = await resolver.GetLookupValues(null);

            Assert.Contains("NL", result.Values);

        }
    }
}

