using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Reflection;
using VxFormGenerator.Components.Plain.Models;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Attributes;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Form.Components.Plain
{
    public class InputSelectWithOptions<TValue> : InputSelect<TValue>, IRenderChildren, IRenderChildrenVxLookupValueKey
    {
        public static Type TypeOfChildToRender => typeof(InputSelectOption<string>);

        public static void RenderChildren(RenderTreeBuilder builder, int index, object dataContext,
            string fieldIdentifier)
        {
            // the builder position is between the builder.OpenComponent() and builder.CloseComponent()
            // This means that the component of InputSelect is added en stil open for changes.
            // We can create a new RenderFragment and set the ChildContent attribute of the InputSelect component
            builder.AddAttribute(index + 1, nameof(ChildContent),
                new RenderFragment(_builder =>
                {
                    // check if the type of the propery is an Enum
                    if (typeof(TValue).IsEnum)
                    {
                        // when type is a enum present them as an <option> element 
                        // by leveraging the component InputSelectOption
                        var values = typeof(TValue).GetEnumValues();


                        foreach (var val in values)
                        {
                            var value = VxSelectItem.ToSelectItem(val as Enum);

                            //  Open the InputSelectOption component
                            _builder.OpenComponent(0, TypeOfChildToRender);

                            // Set the value of the enum as a value and key parameter
                            _builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), value.Label);
                            _builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), value.Key);

                            // Close the component
                            _builder.CloseComponent();
                        }
                   
                    }
                   

                }));

        }

        public static void RenderChildren(RenderTreeBuilder _builder, int index, object dataContext,
            string fieldIdentifier, VxLookupResult<string> vxLookup)
        {
            foreach (var val in vxLookup.Values)
            {
                //  Open the InputSelectOption component
                _builder.OpenComponent(0, TypeOfChildToRender);

                // Set the value of the enum as a value and key parameter
                _builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), val.Value);
                _builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), val.Key);

                // Close the component
                _builder.CloseComponent();
            }
        }

    }
}
