using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using VxFormGenerator.Core;

namespace VxFormGenerator.Form.Components.Plain
{
    public class InputSelectWithOptions<TValue> : InputSelect<TValue>, IRenderChildren
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
                            //  Open the InputSelectOption component
                            _builder.OpenComponent(0, TypeOfChildToRender);

                            // Set the value of the enum as a value and key parameter
                            _builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), val.ToString());
                            _builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), val.ToString());

                            // Close the component
                            _builder.CloseComponent();
                        }
                    }


                }));

        }

    }
}
