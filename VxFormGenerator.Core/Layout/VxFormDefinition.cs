using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VxFormGenerator.Core.Layout
{
    public class VxFormDefinition : Attribute
    {
        public string Name { get; set; }

        public List<VxFormGroup> Groups { get; protected set; } = new List<VxFormGroup>();

        internal static VxFormDefinition CreateFromModel(object model, VxFormLayoutOptions options)
        {
            if (model is ExpandoObject)
            {
                throw new NotSupportedException("ExpandoObject forms are not supported. Build a VxFormMetadataModel with VxFormMetadataBuilder and render it with RenderVxFormMetadata instead.");
            }

            var allProperties = VxHelpers.GetModelProperties(model.GetType());

            var rootFormDefinition = model.GetType().GetCustomAttribute<VxFormDefinition>();

            if (rootFormDefinition == null)
                rootFormDefinition = VxFormDefinition.Create();

            var defaultGroup = VxFormGroup.Create();


            foreach (var prop in allProperties)
            {
                if (VxFormGroup.IsFormGroup(prop))
                {
                    var nestedModel = prop.GetValue(model);
                    var formGroup = VxFormGroup.CreateFromModel(nestedModel, options);
                    formGroup.Label = VxFormGroup.GetFormGroup(prop).Label;

                    rootFormDefinition.Groups.Add(formGroup);
                }
                else
                {
                    VxFormGroup.Add(prop.Name, defaultGroup, model, options);
                }

            }
            rootFormDefinition.Groups.Add(defaultGroup);

            return rootFormDefinition;
        }

        private static VxFormDefinition Create()
        {
            return new VxFormDefinition();
        }
    }
}
