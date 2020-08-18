using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using VxFormGenerator.Components.Plain;

namespace VxFormGenerator
{

    public class FormGeneratorComponentsRepository
    {
        private Dictionary<string, Type> _ComponentDict = new Dictionary<string, Type>();

        public Type _DefaultComponent { get; private set; }
        public Type FormElementComponent { get; private set; }


        public FormGeneratorComponentsRepository(Dictionary<string, Type> componentRegistrations, Type defaultComponent)
        {
            _ComponentDict = componentRegistrations;
            _DefaultComponent = defaultComponent;
            FormElementComponent = typeof(FormElement<>);
        }
        public FormGeneratorComponentsRepository(Dictionary<string, Type> componentRegistrations, Type defaultComponent, Type formElement)
        {
            _ComponentDict = componentRegistrations;
            _DefaultComponent = defaultComponent;
            FormElementComponent = formElement;
        }


        public void RegisterComponent(string key, Type component)
        {
            _ComponentDict.Add(key, component);
        }

        public void RemoveComponent(string key)
        {
            _ComponentDict.Remove(key);
        }

        public Type GetComponent(string key)
        {
            Type outVar = null;

            _ComponentDict.TryGetValue(key, out outVar);

            return outVar ?? _DefaultComponent;
        }

        public void Clear()
        {
            _ComponentDict.Clear();
        }
    }
}
