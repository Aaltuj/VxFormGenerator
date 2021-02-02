using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Attributes;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Form.Components.Plain
{
    public class InputSelectWithOptions<TValue> : InputSelect<TValue>, IRenderChildrenVxLookupValueKey
    {
        public static Type TypeOfChildToRender => typeof(InputSelectOption<string>);

        [Parameter]
        public VxLookupKeyValue KeyValueLookup { get; set; }

        public VxLookupResult<string> LookupValues { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (KeyValueLookup != null)
            {
                LookupValues = await KeyValueLookup.GetLookupValues();
            }

            RenderChildren();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }


        public void RenderChildren()
        {
            ChildContent =
                   new RenderFragment(_builder =>
                   {
                       if (LookupValues == null)
                           return;

                       foreach (var val in LookupValues.Values)
                       {
                           _builder.OpenRegion(200);
                           //  Open the InputSelectOption component
                           _builder.OpenComponent(0, TypeOfChildToRender);

                           // Set the value of the enum as a value and key parameter
                           _builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), val.Value);
                           _builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), val.Key);

                           // Close the component
                           _builder.CloseComponent();
                           _builder.CloseRegion();
                       }



                   });
            // this.StateHasChanged();
        }

    }
}
