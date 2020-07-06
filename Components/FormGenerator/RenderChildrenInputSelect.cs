using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class RenderChildrenInputSelect : IRenderChildren
    {
        public Type TypeToRender =>typeof(InputSelect<>);

        public void RenderChildren<TValue>(RenderTreeBuilder builder, int index, object dataContext,
            PropertyInfo propInfoValue)
        {
            // the builder position is between the builder.OpenComponent() and builder.CloseComponent()
            // This means that the component of InputSelect is added en stil open for changes.
            // We can create a new RenderFragment and set the ChildContent attribute of the InputSelect component
            builder.AddAttribute(index + 1, nameof(InputSelect<TValue>.ChildContent),
                new RenderFragment(builder =>
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
                            builder.OpenComponent(0, typeof(InputSelectOption<string>));

                            // Set the value of the enum as a value and key parameter
                            builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), val.ToString());
                            builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), val.ToString());

                            // Close the component
                            builder.CloseComponent();
                        }
                    }


                }));

        }

    }
}
