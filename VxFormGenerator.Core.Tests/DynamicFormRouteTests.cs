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

            Assert.Equal(6, metadata.Fields.Count);
            Assert.Equal("Name", metadata.Fields[0].Name);
            Assert.Equal("customer-name", metadata.Fields[0].Id);
            Assert.Equal("Feeding", metadata.Fields[0].RowLabel);
            Assert.Equal(typeof(string), metadata.Fields[0].FieldType);
            Assert.True(metadata.Fields[0].IsRequired);
            Assert.Equal(2, metadata.Fields[0].MinLength);
            Assert.Equal(80, metadata.Fields[0].MaxLength);
            Assert.Equal("", metadata.Values["Name"]);
            Assert.Equal(typeof(decimal), metadata.Fields[1].FieldType);
            Assert.Equal(decimal.Zero, metadata.Values["Amount"]);
            Assert.Equal(VxFormFieldKind.Select, metadata.Fields[2].FieldKind);
            Assert.Equal("Bottle", metadata.Values["FoodKind"]);
            Assert.Equal(typeof(int?), metadata.Fields[3].FieldType);
            Assert.Null(metadata.Values["Servings"]);
            Assert.Equal(typeof(DateTime?), metadata.Fields[4].FieldType);
            Assert.Null(metadata.Values["ServedOn"]);
        }

        [Fact]
        public void MetadataRenderer_RendersAndUpdatesFallbackInputs()
        {
            var metadata = VxFormMetadataBuilder.Build(CreateDefinition());

            var component = RenderComponent<RenderVxFormMetadata>(parameters => parameters
                .Add(value => value.Model, metadata));

            var nameInput = component.Find("input[name='Name']");
            var amountInput = component.Find("input[name='Amount']");
            var foodKindSelect = component.Find("select[name='FoodKind']");
            var servingsInput = component.Find("input[name='Servings']");
            var servedOnInput = component.Find("input[name='ServedOn']");
            var firstRow = component.Find(".vx-form-row[data-row-id='1']");

            Assert.Contains("Feeding", firstRow.TextContent);
            Assert.Equal("text", nameInput.GetAttribute("type"));
            Assert.Equal("customer-name", nameInput.GetAttribute("id"));
            Assert.Equal("Name", component.Find("label[for='customer-name']").TextContent);
            Assert.Equal("Session name", nameInput.GetAttribute("placeholder"));
            Assert.NotNull(nameInput.GetAttribute("required"));
            Assert.Equal("2", nameInput.GetAttribute("minlength"));
            Assert.Equal("80", nameInput.GetAttribute("maxlength"));
            Assert.Equal("number", amountInput.GetAttribute("type"));
            Assert.Equal("0", amountInput.GetAttribute("min"));
            Assert.Equal("1000", amountInput.GetAttribute("max"));
            Assert.Contains("col-6", amountInput.ParentElement.GetAttribute("class"));
            Assert.True(firstRow.InnerHtml.IndexOf("Amount") < firstRow.InnerHtml.IndexOf("Name"));
            Assert.Equal("food-kind", foodKindSelect.GetAttribute("id"));
            Assert.Equal(3, foodKindSelect.QuerySelectorAll("option").Length);
            Assert.Empty(component.FindAll("input[name='OtherFood']"));
            Assert.Equal("number", servingsInput.GetAttribute("type"));
            Assert.Equal("date", servedOnInput.GetAttribute("type"));

            nameInput.Change("Runtime name");
            amountInput.Change("12.5");
            foodKindSelect.Change("Solid");
            servingsInput.Change("4");
            servedOnInput.Change("2026-07-05");

            Assert.Equal("Runtime name", metadata.Values["Name"]);
            Assert.Equal(12.5m, metadata.Values["Amount"]);
            Assert.Equal("Solid", metadata.Values["FoodKind"]);
            Assert.Equal(4, metadata.Values["Servings"]);
            Assert.Equal(new System.DateTime(2026, 7, 5), metadata.Values["ServedOn"]);

            servingsInput.Change("");
            servedOnInput.Change("");

            Assert.Null(metadata.Values["Servings"]);
            Assert.Null(metadata.Values["ServedOn"]);

            foodKindSelect.Change("Other");

            var otherFoodInput = component.Find("input[name='OtherFood']");
            otherFoodInput.Change("Warm milk");

            Assert.Equal("Other", metadata.Values["FoodKind"]);
            Assert.Equal("Warm milk", metadata.Values["OtherFood"]);
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
                Id = "customer-name",
                TypeName = "string",
                Label = "Name",
                Placeholder = "Session name",
                RowId = 1,
                RowLabel = "Feeding",
                ColSpan = 6,
                Order = 20,
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
                RowLabel = "Feeding",
                ColSpan = 6,
                Order = 10,
                RangeMinimum = "0",
                RangeMaximum = "1000"
            });

            var foodKind = new VxFormModelPropertyDefinition
            {
                Name = "FoodKind",
                Id = "food-kind",
                TypeName = "string",
                Label = "Food kind",
                FieldKind = VxFormFieldKind.Select
            };

            foodKind.Options.Add(new VxFormLookupOption { Value = "Bottle", Label = "Bottle", IsSelected = true });
            foodKind.Options.Add(new VxFormLookupOption { Value = "Solid", Label = "Solid food" });
            foodKind.Options.Add(new VxFormLookupOption { Value = "Other", Label = "Other" });
            definition.Properties.Add(foodKind);

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "OtherFood",
                TypeName = "string",
                Label = "Other food",
                RowId = 2,
                ColSpan = 6,
                VisibilityRule = new VxFormVisibilityRule
                {
                    SourceField = "FoodKind",
                    EqualsValue = "Other"
                }
            });

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "Servings",
                TypeName = "int?",
                Label = "Servings",
                RowId = 2,
                ColSpan = 3
            });

            definition.Properties.Add(new VxFormModelPropertyDefinition
            {
                Name = "ServedOn",
                TypeName = "datetime?",
                Label = "Served on",
                RowId = 2,
                ColSpan = 3
            });

            return definition;
        }
    }
}
