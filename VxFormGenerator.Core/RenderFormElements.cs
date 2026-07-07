using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Render;
using VxFormGenerator.Settings;

namespace VxFormGenerator.Core
{
    public class RenderFormElements : OwningComponentBase
    {
        /// <summary>
        /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
        /// </summary>
        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        [Inject]
        IFormGeneratorOptions FormGeneratorOptions { get; set; }

        [Parameter] public Layout.VxFormLayoutOptions FormLayoutOptions { get; set; }

        [Parameter] public RenderFragment<VxFormFieldTemplateContext> FieldTemplate { get; set; }

        /// <summary>
        /// Override the default render method, determining if the <see cref="EditContext.Model"/> 
        /// is a typed model class.
        /// </summary>
        /// <param name="builder">Instance of the page builder</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var formDefinition = VxFormDefinition.CreateFromModel(CascadedEditContext.Model, FormLayoutOptions);

            builder.OpenComponent(0, typeof(CascadingValue<RenderFragment<VxFormFieldTemplateContext>>));
            builder.AddAttribute(1, nameof(CascadingValue<RenderFragment<VxFormFieldTemplateContext>>.Value), FieldTemplate);
            builder.AddAttribute(2, nameof(CascadingValue<RenderFragment<VxFormFieldTemplateContext>>.IsFixed), true);
            builder.AddAttribute(3, nameof(CascadingValue<RenderFragment<VxFormFieldTemplateContext>>.ChildContent), new RenderFragment(templateBuilder =>
            {
                templateBuilder.OpenComponent(0, typeof(CascadingValue<VxFormLayoutOptions>));
                templateBuilder.AddAttribute(1, nameof(CascadingValue<VxFormLayoutOptions>.Value), FormLayoutOptions);
                templateBuilder.AddAttribute(2, nameof(CascadingValue<VxFormLayoutOptions>.ChildContent), new RenderFragment(_builder =>
                {
                    var counter = 2;

                    foreach (var group in formDefinition.Groups)
                    {
                        _builder.OpenComponent(counter++, FormGeneratorOptions.FormGroupElement);
                        _builder.AddAttribute(counter++, nameof(VxFormGroupBase.FormGroupDefinition), group);
                        _builder.CloseComponent();
                    }

                }));

                templateBuilder.CloseComponent();
            }));

            builder.CloseComponent();

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetupFramework();
        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetupFramework();
        }

        private void SetupFramework()
        {
            if (FormGeneratorOptions.FieldCssClassProvider != null)
            {
                var provider = FormGeneratorOptions.FieldCssClassProvider as VxFormCssClassProviderBase;
                // Set the options in the custom FieldCssClassProvider
                provider.FormLayoutOptions = FormLayoutOptions;

                CascadedEditContext.SetFieldCssClassProvider(provider);
            }
            if (FormLayoutOptions == null)
            {
                FormLayoutOptions = (VxFormLayoutOptions)ScopedServices.GetService(typeof(Layout.VxFormLayoutOptions));
            }
        }



    }

}
