#pragma checksum "D:\Dev\BlazorFormGeneratorDemo\VxFormGenerator\Components\InputCheckboxMultiple.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5d4eea44a9f215e983e814e5cda001f80149355f"
// <auto-generated/>
#pragma warning disable 1591
namespace VxFormGenerator.Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\Dev\BlazorFormGeneratorDemo\VxFormGenerator\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Dev\BlazorFormGeneratorDemo\VxFormGenerator\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
    public partial class InputCheckboxMultiple<T> : InputCheckboxMultipleComponent<T>
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __Blazor.VxFormGenerator.Components.InputCheckboxMultiple.TypeInference.CreateCascadingValue_0(__builder, 0, 1, 
#nullable restore
#line 4 "D:\Dev\BlazorFormGeneratorDemo\VxFormGenerator\Components\InputCheckboxMultiple.razor"
                        this

#line default
#line hidden
#nullable disable
            , 2, "InputCheckboxMultiple", 3, (__builder2) => {
                __builder2.AddMarkupContent(4, "\r\n    ");
                __builder2.AddContent(5, 
#nullable restore
#line 5 "D:\Dev\BlazorFormGeneratorDemo\VxFormGenerator\Components\InputCheckboxMultiple.razor"
     ChildContent

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(6, "\r\n");
            }
            );
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.VxFormGenerator.Components.InputCheckboxMultiple
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateCascadingValue_0<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TValue __arg0, int __seq1, global::System.String __arg1, int __seq2, global::Microsoft.AspNetCore.Components.RenderFragment __arg2)
        {
        __builder.OpenComponent<global::Microsoft.AspNetCore.Components.CascadingValue<TValue>>(seq);
        __builder.AddAttribute(__seq0, "Value", __arg0);
        __builder.AddAttribute(__seq1, "Name", __arg1);
        __builder.AddAttribute(__seq2, "ChildContent", __arg2);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
