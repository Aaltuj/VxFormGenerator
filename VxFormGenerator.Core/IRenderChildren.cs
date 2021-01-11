using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Core
{
    /// <summary>
    /// Helper interface for rendering values in components, needs to be non-generic for the form generator
    /// </summary>
    public interface IRenderChildren
    {

        /// <summary>
        /// Function that will render the children for <see cref="TypeToRender"/>
        /// </summary>
        /// <typeparam name="TElement">The element type of the <see cref="TypeToRender"/></typeparam>
        /// <param name="builder">The builder for rendering a tree</param>
        /// <param name="index">The index of the element</param>
        /// <param name="dataContext">The model for the form</param>
        /// <param name="propInfoValue">The property that is filled by the <see cref="FormElement"/></param>
        public static void RenderChildren(RenderTreeBuilder builder, int index, object dataContext,
            string fieldIdentifier) => throw new NotImplementedException();

    }

    /// <summary>
    /// Helper interface for that allows a derived component set the component that needs to render. 
    /// Useful for components that render children and should allow a different styling without changing logic
    /// </summary>
    public interface IRenderChildrenSwapable: IRenderChildren
    {

        /// <summary>
        /// Function that will render the children for <see cref="TypeToRender"/>
        /// </summary>
        /// <typeparam name="TElement">The element type of the <see cref="TypeToRender"/></typeparam>
        /// <param name="builder">The builder for rendering a tree</param>
        /// <param name="index">The index of the element</param>
        /// <param name="dataContext">The model for the form</param>
        /// <param name="propInfoValue">The property that is filled by the <see cref="FormElement"/></param>
        /// <param name="typeOfChildToRender">The type of the child that should be rendered</param>
        public static void RenderChildren(RenderTreeBuilder builder, int index, object dataContext,
            string fieldIdentifier,
            Type typeOfChildToRender) => throw new NotImplementedException();

    }

    /// <summary>
    /// Add interface to the component that allows rendering of the <see cref="VxLookupKeyValue"/>
    /// </summary>
    public interface IRenderChildrenVxLookupValueKey
    {
        /// <summary>
        /// Function that will render the children for <see cref="TypeToRender"/>
        /// </summary>
        /// <typeparam name="TElement">The element type of the <see cref="TypeToRender"/></typeparam>
        /// <param name="builder">The builder for rendering a tree</param>
        /// <param name="index">The index of the element</param>
        /// <param name="dataContext">The model for the form</param>
        /// <param name="propInfoValue">The property that is filled by the <see cref="FormElement"/></param>
        /// <param name="typeOfChildToRender">The type of the child that should be rendered</param>
        public static void RenderChildren(RenderTreeBuilder builder, int index, object dataContext,
            string fieldIdentifier,
            VxLookupKeyValue vxLookup) => throw new NotImplementedException();
 
    }
}