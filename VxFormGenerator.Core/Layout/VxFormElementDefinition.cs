
using System;
using System.Reflection;
using VxFormGenerator.Core.Render;

namespace VxFormGenerator.Core.Layout
{
    public class VxFormElementDefinition
    {


        public string Name { get; private set; }
        public VxFormElementLayoutAttribute RenderOptions { get; set; }
        public object Model { get; private set; }
        public bool HasLookup
        {
            get
            {
                return Model.GetType().GetCustomAttribute<VxLookupKeyValue>() != null;
            }
        }

        public VxFormElementDefinition(string fieldname, VxFormElementLayoutAttribute layoutAttr, object modelInstance)
        {
            RenderOptions = layoutAttr;
            Name = fieldname;
            Model = modelInstance;
        }


        internal static VxFormElementDefinition Create(string fieldname, VxFormElementLayoutAttribute layoutAttr, object modelInstance, VxFormLayoutOptions options)
        {
            return new VxFormElementDefinition(fieldname, layoutAttr, modelInstance);
        }
    }
}
