using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using VxFormGenerator.Core.Dynamic;
using VxFormGeneratorDemo.Shared.Pages;
using Xunit;

namespace VxFormGenerator.Core.Tests
{
    public class DynamicFormRouteTests : TestContext
    {
        [Fact]
        public void DynamicForm_HasWasmDiscoverableRoute()
        {
            var route = typeof(DynamicForm)
                .GetCustomAttributes(typeof(RouteAttribute), inherit: false)
                .Cast<RouteAttribute>()
                .SingleOrDefault(attribute => attribute.Template == "/dynamic-form");

            Assert.NotNull(route);
        }

        [Fact]
        public void DynamicForm_RendersMetadataGeneratedForm()
        {
            var component = RenderComponent<DynamicForm>();

            Assert.Contains("Dynamic Metadata Form Demo", component.Markup);
            Assert.Contains("Name: String", component.Markup);
            Assert.Contains("Amount: Decimal", component.Markup);
            Assert.Contains("input", component.Markup);
            Assert.Contains("name=\"Name\"", component.Markup);
            Assert.Contains("name=\"Amount\"", component.Markup);
            Assert.Contains("public class FeedingSessionForm", component.Markup);
            Assert.Contains("[Required]", component.Markup);
            Assert.Contains("[VxFormElementLayout", component.Markup);
        }

        [Fact]
        public void MetadataBuilder_BuildsWasmFallbackModel()
        {
            var definition = CreateDefinition();

            var metadata = VxFormMetadataBuilder.Build(definition);

            Assert.Equal(2, metadata.Fields.Count);
            Assert.Equal("Name", metadata.Fields[0].Name);
            Assert.Equal(typeof(string), metadata.Fields[0].FieldType);
            Assert.True(metadata.Fields[0].IsRequired);
            Assert.Equal(2, metadata.Fields[0].MinLength);
            Assert.Equal(80, metadata.Fields[0].MaxLength);
            Assert.Equal("", metadata.Values["Name"]);
            Assert.Equal(typeof(decimal), metadata.Fields[1].FieldType);
            Assert.Equal(decimal.Zero, metadata.Values["Amount"]);
        }

        [Fact]
        public void MetadataRenderer_RendersAndUpdatesFallbackInputs()
        {
            var metadata = VxFormMetadataBuilder.Build(CreateDefinition());

            var component = RenderComponent<RenderVxFormMetadata>(parameters => parameters
                .Add(value => value.Model, metadata));

            var nameInput = component.Find("input[name='Name']");
            var amountInput = component.Find("input[name='Amount']");

            Assert.Equal("text", nameInput.GetAttribute("type"));
            Assert.Equal("Session name", nameInput.GetAttribute("placeholder"));
            Assert.NotNull(nameInput.GetAttribute("required"));
            Assert.Equal("2", nameInput.GetAttribute("minlength"));
            Assert.Equal("80", nameInput.GetAttribute("maxlength"));
            Assert.Equal("number", amountInput.GetAttribute("type"));
            Assert.Equal("0", amountInput.GetAttribute("min"));
            Assert.Equal("1000", amountInput.GetAttribute("max"));

            nameInput.Change("Runtime name");
            amountInput.Change("12.5");

            Assert.Equal("Runtime name", metadata.Values["Name"]);
            Assert.Equal(12.5m, metadata.Values["Amount"]);
        }

        private static VxFormModelDefinition CreateDefinition()
        {
            var definition = new VxFormModelDefinition
            {
                Namespace = "VxFormGeneratorDemo.Generated",
                ClassName = "FeedingSessionForm"
            };

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "Name",
                TypeName = "string",
                Label = "Name",
                Placeholder = "Session name",
                RowId = 1,
                ColSpan = 6,
                IsRequired = true,
                MinLength = 2,
                MaxLength = 80,
                DefaultValueExpression = "string.Empty"
            });

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "Amount",
                TypeName = "decimal",
                Label = "Amount",
                RowId = 1,
                ColSpan = 6,
                RangeMinimum = "0",
                RangeMaximum = "1000"
            });

            return definition;
        }
    }
}
