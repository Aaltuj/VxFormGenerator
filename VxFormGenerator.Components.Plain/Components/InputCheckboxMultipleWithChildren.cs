using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Render;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Form.Components.Plain
{
    [VxDataTypeRegistration(SupportedDataType = typeof(IEnumerable))]
    public class InputCheckboxMultipleWithChildren<TValue> : InputCheckboxMultiple<TValue>, IRenderChildrenVxLookupValueKey
    {
        [Parameter]
        public VxLookupKeyValue KeyValueLookup { get; set; }
        public VxLookupResult<string> LookupValues { get; set; }

        protected static Type TypeOfChildToRender { get => typeof(VxInputCheckbox); }

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

                  

                   // adding a Class attribute containing the supported data-types this allow 
                   // the component to call a function that ties the properties type to the supported type, and returns the ValueChanged and ValueExpression
                   
                   foreach (var val in VxSelectItem.ToSelectItems(LookupValues.Values))
                   {
                       var _index = 0;

                       //  Open the InputSelectOption component
                       _builder.OpenComponent(_index++, TypeOfChildToRender);

                       // Set the value of the enum as a value and key parameter
                       _builder.AddAttribute(_index++, nameof(VxInputCheckbox.Value), val.Selected);

                       // Create the handler for ValueChanged. This wil update the model instance with the input
                       _builder.AddAttribute(_index++, nameof(ValueChanged),
                              Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(
                                  EventCallback.Factory.Create<bool>(
                                      val, EventCallback.Factory.
                                      CreateInferred(val.Selected, __value => val.Selected = __value, val.Selected))));
                      

                       // Create an expression to set the ValueExpression-attribute.
                       var constant = Expression.Constant(val, val.GetType());
                       var exp = Expression.Property(constant, nameof(VxSelectItem.Selected));
                       var lamb = Expression.Lambda<Func<bool>>(exp);
                       _builder.AddAttribute(_index++, nameof(InputBase<bool>.ValueExpression), lamb);

                       _builder.AddAttribute(_index++, nameof(VxInputCheckbox.Label), val.Label);

                       // Close the component
                       _builder.CloseComponent();
                   }


               });
        }
    }
}
