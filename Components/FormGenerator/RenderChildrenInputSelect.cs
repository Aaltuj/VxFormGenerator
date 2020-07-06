using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class RenderChildrenInputSelect : ICanRenderChildren
    {
        public Type TypeToRender =>typeof(InputSelect<>);

        public void RenderChildren<TValue>(RenderTreeBuilder builder, int index, object dataContext,
            PropertyInfo propInfoValue)
        {

            builder.AddAttribute(5, nameof(InputSelect<TValue>.ChildContent),
                new RenderFragment(builder =>
                {
                    if (typeof(TValue).IsEnum)
                    {
                        // when type is a enum present them as an <option> element by levering the component InputSelectOption
                        var values = typeof(TValue).GetEnumValues();
                        foreach (var val in values)
                        {
                            builder.OpenComponent(0, typeof(InputSelectOption<string>));

                            builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), val.ToString());
                            builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), val.ToString());

                            builder.CloseComponent();
                        }
                    }


                }));

        }

    }
}
