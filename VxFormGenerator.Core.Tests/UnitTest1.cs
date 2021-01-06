using Bunit;
using System;
using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Layout;
using VxFormGeneratorDemoData;
using Xunit;

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

            var value = VisualFeedbackValidationPolicy.VALID_AND_INVALID;
            var attribute = value.GetAttribute<DisplayAttribute>();
            Assert.Equal("When valid or invalid", attribute.Name);

        }
    }
}

