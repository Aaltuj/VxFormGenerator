using Bunit;
using System;
using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Attributes;
using VxFormGenerator.Core.Layout;
using VxFormGeneratorDemoData;
using Xunit;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using VxFormGenerator.Core.Repository.Registration;

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
            var result = await resolver.GetLookupValues();

            Assert.Contains("NL", result.Values);

        }

        [Fact]
        public void ConvertOldRegistrionList()
        {

            var registrationDict = new Dictionary<Type, Type>()
                  {
                    { typeof(string),          typeof(InputText) },
                    { typeof(DateTime),        typeof(InputDate<>) }
            };

            var result = VxDataTypeComponentRegistration.CreateRegistrationList(registrationDict);

            var stringRegistration = result.GetValueOrDefault(typeof(string));
            var stringRegistrationValue = stringRegistration[0];

            var dateRegistration = result.GetValueOrDefault(typeof(DateTime));
            var dateRegistrationValue = dateRegistration[0];

            Assert.Equal(1, stringRegistration.Count);
            Assert.Equal(typeof(string), stringRegistrationValue.SupportedDataType);
            Assert.Equal(typeof(InputText), stringRegistrationValue.Component);

            Assert.Equal(1, dateRegistration.Count);
            Assert.Equal(typeof(DateTime), dateRegistrationValue.SupportedDataType);
            Assert.Equal(typeof(InputDate<>), dateRegistrationValue.Component);

        }


        [Fact]
        public void GetAllRegisteredFormElements()
        {
            var repo = new VxFormGenerator.Repository.Plain.VxComponentsRepository();

        }
    }
}

