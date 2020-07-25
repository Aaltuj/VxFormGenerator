using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace VxFormGenerator
{
    public abstract class VxInputBase<TValue> : InputBase<TValue>
    {
        private string id = Guid.NewGuid().ToString();

        [Parameter] public string Id { get => id; set => id = value; }

    }
}
