using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    /// <summary>
    /// Helper interface for rendering values in components, needs to be non-generic for the form generator
    /// </summary>
    public interface ICanRenderChildren
    {
        Type TypeToRender { get; }
        void RenderChildren<TElement>(RenderTreeBuilder builder, int index, object dataContext,
            PropertyInfo propInfoValue);
    }
}