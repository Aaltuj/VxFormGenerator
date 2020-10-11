using System;
using System.Collections.Generic;

namespace VxFormGenerator.Core.Repository
{

    public class FormGeneratorComponentsRepository<TKey> : IFormGeneratorComponentsRepository
    {
        protected Dictionary<TKey, Type> _ComponentDict = new Dictionary<TKey, Type>();

        public Type _DefaultComponent { get; protected set; }

        public FormGeneratorComponentsRepository()
        {

        }

        public FormGeneratorComponentsRepository(Dictionary<TKey, Type> componentRegistrations)
        {
            _ComponentDict = componentRegistrations;
        }
        public FormGeneratorComponentsRepository(Dictionary<TKey, Type> componentRegistrations, Type defaultComponent)
        {
            _ComponentDict = componentRegistrations;
            _DefaultComponent = defaultComponent;
        }


        protected void RegisterComponent(TKey key, Type component)
        {
            _ComponentDict.Add(key, component);
        }

        protected void RemoveComponent(TKey key)
        {
            _ComponentDict.Remove(key);
        }

        protected virtual Type GetComponent(TKey key)
        {
            var found = _ComponentDict.TryGetValue(key, out Type outVar);

            return found ? outVar : _DefaultComponent;
        }

        public void Clear()
        {
            _ComponentDict.Clear();
        }

        public void RegisterComponent(object key, Type component)
        {
            RegisterComponent((TKey) key, component);
        }

        public void RemoveComponent(object key)
        {
            RemoveComponent((TKey) key);
        }

        public Type GetComponent(object key)
        {
            return GetComponent((TKey) key);
        }
    }


}
