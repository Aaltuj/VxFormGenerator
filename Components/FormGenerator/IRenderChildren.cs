using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    /// <summary>
    /// Helper interface for rendering values in components, needs to be non-generic for the form generator
    /// </summary>
    public interface IRenderChildren
    {
        /// <summary>
        /// Let the form generator know what element to render    
        /// </summary>
        Type TypeToRender { get; }
        /// <summary>
        /// Function that will render the children for <see cref="TypeToRender"/>
        /// </summary>
        /// <typeparam name="TElement">The element type of the <see cref="TypeToRender"/></typeparam>
        /// <param name="builder">The builder for rendering a tree</param>
        /// <param name="index">The index of the element</param>
        /// <param name="dataContext">The model for the form</param>
        /// <param name="propInfoValue">The property that is filled by the <see cref="FormElement"/></param>
        void RenderChildren<TElement>(RenderTreeBuilder builder, int index, object dataContext,
            PropertyInfo propInfoValue);
    }
}