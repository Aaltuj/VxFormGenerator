using VxFormGenerator.Core.Scaffolding;
using VxFormGeneratorDemoData;
using Xunit;

namespace VxFormGenerator.Core.Tests
{
    public class RazorComponentScaffoldTests
    {
        [Fact]
        public void Generate_CreatesEditableRazorComponentWithFieldTemplate()
        {
            var source = VxFormRazorComponentSourceGenerator.Generate(typeof(AddressViewModel));

            Assert.Contains("<EditForm Model=\"Model\" OnValidSubmit=\"HandleValidSubmit\">", source);
            Assert.Contains("<FieldTemplate Context=\"field\">", source);
            Assert.Contains("@field.Input", source);
            Assert.Contains("@field.ValidationMessage", source);
            Assert.Contains("EventCallback<VxFormGeneratorDemoData.AddressViewModel> ValidSubmit", source);
            Assert.Contains("// - SurName (System.String): Firstname", source);
        }

        [Fact]
        public void Generate_CanCreateSimpleRenderFormElementsComponent()
        {
            var source = VxFormRazorComponentSourceGenerator.Generate(typeof(AddressViewModel), new VxFormRazorComponentScaffoldOptions
            {
                IncludeFieldTemplate = false
            });

            Assert.Contains("<RenderFormElements FormLayoutOptions=\"Options\" />", source);
            Assert.DoesNotContain("<FieldTemplate", source);
        }
    }
}
