using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Settings.Bootstrap;
using VxFormGeneratorDemo.Shared.Pages;
using Xunit;

namespace VxFormGenerator.Core.Tests
{
    public class CustomFieldTemplateRouteTests : BunitContext
    {
        public CustomFieldTemplateRouteTests()
        {
            Services.AddVxFormGenerator();
        }

        [Fact]
        public void CustomFieldTemplate_HasDiscoverableRoute()
        {
            var route = typeof(CustomFieldTemplate)
                .GetCustomAttributes(typeof(RouteAttribute), inherit: false)
                .Cast<RouteAttribute>()
                .SingleOrDefault(attribute => attribute.Template == "/custom-field-template");

            Assert.NotNull(route);
        }

        [Fact]
        public void CustomFieldTemplate_RendersCustomMarkupWithGeneratedInput()
        {
            var component = Render<CustomFieldTemplate>();

            Assert.Contains("Custom Field Template Demo", component.Markup);
            Assert.NotEmpty(component.FindAll("[data-field-name='SurName']"));
            Assert.NotNull(component.Find("[data-field-name='SurName'] input"));
            Assert.Contains("Generated inputs, custom HTML", component.Markup);
        }
    }
}
