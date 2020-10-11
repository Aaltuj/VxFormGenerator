using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace VxFormGenerator.Core
{
    /// <summary>
    /// Extended version of the <see cref="InputBase{TValue}"/> allows for generated HTML ID attributes
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class VxInputBase<TValue> : InputBase<TValue>
    {
        private string id = Guid.NewGuid().ToString();

        /// <summary>
        /// The html id attribute that could be used for the element
        /// </summary>
        [Parameter] public string Id { get => id; set => id = value; }

    }
}
