using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public static class FormGeneratorChildrenRenderMethods
    {
        public static void RenderInputSelectChilderen<T>(RenderTreeBuilder builder, int index)
        {
            
                builder.AddAttribute(5, nameof(InputSelect<T>.ChildContent),
                    new RenderFragment(builder =>
                    {
                        if (typeof(T).IsEnum)
                        {
                            var values = typeof(T).GetEnumValues();
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
