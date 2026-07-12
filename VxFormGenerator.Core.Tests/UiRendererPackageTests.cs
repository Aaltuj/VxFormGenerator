using System;
using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Core;
using VxFormGenerator.Form.Components.Bulma;
using VxFormGenerator.Form.Components.Tailwind;
using VxFormGenerator.Render.Bulma;
using VxFormGenerator.Render.Tailwind;
using VxFormGenerator.Repository.Bulma;
using VxFormGenerator.Repository.Tailwind;
using VxFormGenerator.Settings.Bulma;
using VxFormGenerator.Settings.Tailwind;
using Xunit;

namespace VxFormGenerator.Core.Tests
{
    public class UiRendererPackageTests : BunitContext
    {
        [Fact]
        public void BulmaOptionsUseBulmaRendererAndValidationClasses()
        {
            var options = new VxBulmaFormOptions();

            Assert.Equal(typeof(BulmaFormElement<>), options.FormElementComponent);

            var provider = Assert.IsType<VxBulmaFormCssClassProvider>(options.FieldCssClassProvider);
            Assert.Equal("is-success", provider.CssClassAttribute.Valid);
            Assert.Equal("is-danger", provider.CssClassAttribute.Invalid);
        }

        [Fact]
        public void TailwindOptionsUseTailwindRendererAndValidationClasses()
        {
            var options = new VxTailwindFormOptions();

            Assert.Equal(typeof(TailwindFormElement<>), options.FormElementComponent);

            var provider = Assert.IsType<VxTailwindFormCssClassProvider>(options.FieldCssClassProvider);
            Assert.Contains("border-green-500", provider.CssClassAttribute.Valid);
            Assert.Contains("border-red-500", provider.CssClassAttribute.Invalid);
        }

        [Fact]
        public void BulmaRepositoryMapsCoreFieldTypesToBulmaComponents()
        {
            var repository = new VxBulmaRepository();

            Assert.Equal(typeof(InputText), repository.GetComponent(typeof(string)));
            Assert.Equal(typeof(BulmaInputCheckbox), repository.GetComponent(typeof(bool)));
            Assert.Equal(typeof(BulmaInputSelectWithOptions<>), repository.GetComponent(typeof(DemoChoice)));
            Assert.Equal(typeof(BulmaInputCheckboxMultiple<>), repository.GetComponent(typeof(ValueReferences)));
        }

        [Fact]
        public void TailwindRepositoryMapsCoreFieldTypesToTailwindComponents()
        {
            var repository = new VxTailwindRepository();

            Assert.Equal(typeof(InputText), repository.GetComponent(typeof(string)));
            Assert.Equal(typeof(TailwindInputCheckbox), repository.GetComponent(typeof(bool)));
            Assert.Equal(typeof(TailwindInputSelectWithOptions<>), repository.GetComponent(typeof(DemoChoice)));
            Assert.Equal(typeof(TailwindInputCheckboxMultiple<>), repository.GetComponent(typeof(ValueReferences)));
        }



        [Fact]
        public void BulmaRendererUsesTextInputDefaultsOnlyForPlainInputComponents()
        {
            global::VxFormGenerator.Settings.Bulma.ServiceCollectionExtensions.AddVxFormGenerator(Services);

            var component = Render<UiRendererTestHost>();

            Assert.Contains("input", component.Find("input:not([type])").ClassList);
            Assert.Contains("input", component.Find("input[type='number']").ClassList);
            Assert.DoesNotContain("input", component.Find("input[type='checkbox']").ClassList);
            Assert.Contains("checkbox", component.Find("input[type='checkbox']").ClassList);
            Assert.Contains("select", component.Find("select").ClassList);
        }

        [Fact]
        public void TailwindRendererUsesTextInputDefaultsOnlyForPlainInputComponents()
        {
            global::VxFormGenerator.Settings.Tailwind.ServiceCollectionExtensions.AddVxFormGenerator(Services);

            var component = Render<UiRendererTestHost>();

            Assert.Contains("w-full", component.Find("input:not([type])").ClassList);
            Assert.Contains("w-full", component.Find("input[type='number']").ClassList);
            Assert.DoesNotContain("w-full", component.Find("input[type='checkbox']").ClassList);
            Assert.Contains("h-4", component.Find("input[type='checkbox']").ClassList);
            Assert.Contains("w-full", component.Find("select").ClassList);
        }

        private enum DemoChoice
        {
            First,
            Second
        }
    }
}
