using Microsoft.AspNetCore.Components;
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
        protected static Type TypeOfChildToRender { get; }
        public void RenderChildren() => throw new NotImplementedException();

    }

    /// <summary>
    /// Add interface to the component that allows rendering of the <see cref="VxLookupKeyValue"/>
    /// </summary>
    public interface IRenderChildrenVxLookupValueKey : IRenderChildren
    {
        [Parameter]
        public VxLookupKeyValue KeyValueLookup { get; set; }

        public VxLookupResult<string> LookupValues { get; set; }

    }
}