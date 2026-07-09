using Bunit;
using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Settings.Plain;
using Xunit;

namespace VxFormGenerator.Core.Tests
{
    public class RenderFormElementsTemplateTests : BunitContext
    {
        public RenderFormElementsTemplateTests()
        {
            Services.AddVxFormGenerator();
        }

        [Fact]
        public void RenderFormElements_UsesFieldTemplateMarkup()
        {
            var component = Render<TemplatedRenderFormElementsTestHost>();

            var surnameField = component.Find(".custom-field[data-field-name='SurName']");

            Assert.Equal("Firstname", surnameField.QuerySelector(".custom-label").TextContent);
            Assert.NotNull(surnameField.QuerySelector(".custom-input input"));
            Assert.NotNull(surnameField.QuerySelector(".custom-validation"));
        }

        [Fact]
        public void RenderFormElements_FieldTemplateKeepsGeneratedInputBinding()
        {
            var component = Render<TemplatedRenderFormElementsTestHost>();
            var surnameInput = component.Find(".custom-field[data-field-name='SurName'] input");

            surnameInput.Change("Custom markup value");

            Assert.Equal("Custom markup value", component.Instance.Model.SurName);
        }

        [Fact]
        public void RenderFormElements_LeftOrientationAddsFieldLabelsAboveInputs()
        {
            var component = Render<LeftOrientationRenderFormElementsTestHost>();

            Assert.NotNull(component.Find(".vx-form-left-row-label:not(.vx-form-explicit-row-label)"));
            Assert.NotNull(component.Find(".vx-form-explicit-row-label"));
            Assert.Contains(component.FindAll(".vx-form-left-field-label"), label => label.TextContent == "Firstname");
            Assert.Contains(component.FindAll(".vx-form-left-field-label"), label => label.TextContent == "Lastname");
            Assert.DoesNotContain(component.FindAll(".vx-form-left-field-label"), label => label.TextContent == "Street");
            Assert.DoesNotContain(component.FindAll(".vx-form-left-field-label"), label => label.TextContent == "Number");
        }
    }
}
