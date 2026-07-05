using Bunit;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using VxFormGenerator.Core.Dynamic;
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
            var definition = VxFormDefinition.CreateFromModel(new AddressViewModel(), new VxFormLayoutOptions());

            Assert.NotEmpty(definition.Groups);

        }

        [Fact]
        public void EnumExtentionGetAttribute()
        {

            var value = VisualFeedbackValidationPolicy.VALID_AND_INVALID;
            var attribute = value.GetAttribute<DisplayAttribute>();
            Assert.Equal("When valid or invalid", attribute.Name);

        }

        [Fact]
        public void GenerateModelSource_WithValidationAndLayoutAttributes()
        {
            var definition = new VxFormModelDefinition
            {
                Namespace = "Demo.Generated",
                ClassName = "Customer Form"
            };

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "First Name",
                TypeName = "string",
                Label = "First name",
                Placeholder = "Your first name",
                RowId = 1,
                ColSpan = 6,
                Order = 10,
                IsRequired = true,
                MinLength = 2,
                MaxLength = 40,
                DefaultValueExpression = "string.Empty"
            });

            var source = VxFormModelSourceGenerator.Generate(definition);

            Assert.Contains("namespace Demo.Generated", source);
            Assert.Contains("public class Customer_Form", source);
            Assert.Contains("[Display(Name = \"First name\", Order = 10)]", source);
            Assert.Contains("[VxFormElementLayout(RowId = 1, ColSpan = 6, Label = \"First name\", Placeholder = \"Your first name\", Order = 10)]", source);
            Assert.Contains("[Required]", source);
            Assert.Contains("[StringLength(40, MinimumLength = 2)]", source);
            Assert.Contains("public string First_Name { get; set; } = string.Empty;", source);
        }

        [Fact]
        public void BuildRuntimeModel_WithValidationAndLayoutAttributes()
        {
            var definition = new VxFormModelDefinition
            {
                Namespace = "Demo.Generated",
                ClassName = "CustomerForm"
            };

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "FirstName",
                TypeName = "string",
                Label = "First name",
                Placeholder = "Your first name",
                RowId = 1,
                ColSpan = 6,
                Order = 10,
                IsRequired = true,
                MinLength = 2,
                MaxLength = 40
            });

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "Amount",
                RuntimeType = typeof(decimal?),
                Label = "Amount",
                RangeMinimum = "0",
                RangeMaximum = "1000"
            });

            var modelType = VxFormRuntimeModelBuilder.BuildType(definition);
            var instance = Activator.CreateInstance(modelType);
            var firstNameProperty = modelType.GetProperty("FirstName");
            var amountProperty = modelType.GetProperty("Amount");

            firstNameProperty.SetValue(instance, "Alex");

            Assert.Equal("Demo.Generated.CustomerForm", modelType.FullName);
            Assert.Equal("Alex", firstNameProperty.GetValue(instance));
            Assert.Equal(typeof(decimal?), amountProperty.PropertyType);
            Assert.Equal("First name", firstNameProperty.GetCustomAttribute<DisplayAttribute>().GetName());
            Assert.NotNull(firstNameProperty.GetCustomAttribute<RequiredAttribute>());
            Assert.Equal(2, firstNameProperty.GetCustomAttribute<StringLengthAttribute>().MinimumLength);
            Assert.Equal(40, firstNameProperty.GetCustomAttribute<StringLengthAttribute>().MaximumLength);
            Assert.Equal(1, firstNameProperty.GetCustomAttribute<VxFormElementLayoutAttribute>().RowId);
            Assert.Equal(6, firstNameProperty.GetCustomAttribute<VxFormElementLayoutAttribute>().ColSpan);
            Assert.NotNull(amountProperty.GetCustomAttribute<RangeAttribute>());
        }
    }
}
