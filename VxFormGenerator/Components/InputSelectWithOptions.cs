using VxFormGenerator.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace VxFormGenerator.Components
{
    public class InputSelectWithOptions<TValue> : InputSelect<TValue>, IRenderChildren
    {

        public void RenderChildren(RenderTreeBuilder builder, int index, object dataContext,
            PropertyInfo propInfoValue)
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
                            _builder.OpenComponent(0, typeof(InputSelectOption<string>));

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
