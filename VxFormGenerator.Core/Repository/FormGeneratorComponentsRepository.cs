using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Core.Repository
{

    public class FormGeneratorComponentsRepository<TKey, TRegistrationAttribute> : IFormGeneratorComponentsRepository, IChangeFormGeneratorComponentsRepository<TKey> where TRegistrationAttribute : IVxComponentAttributeRegistration<TKey>
    {
        protected Dictionary<TKey, IList<IVxComponentRegistration<TKey>>> _ComponentDict = new Dictionary<TKey, IList<IVxComponentRegistration<TKey>>>();

        public Type _DefaultComponent { get; protected set; }

        public FormGeneratorComponentsRepository()
        {

        }

        public FormGeneratorComponentsRepository(Dictionary<TKey, IList<IVxComponentRegistration<TKey>>> componentRegistrations)
        {
            _ComponentDict = componentRegistrations;
        }
        public FormGeneratorComponentsRepository(Dictionary<TKey, IList<IVxComponentRegistration<TKey>>> componentRegistrations, Type defaultComponent)
        {
            _ComponentDict = componentRegistrations;
            _DefaultComponent = defaultComponent;
        }


        protected void RegisterComponent(TKey key, IVxComponentRegistration<TKey> registration)
        {
            if (_ComponentDict.ContainsKey(key))
            {
                var dataRegistration = _ComponentDict.GetValueOrDefault(key);
                dataRegistration.Add(registration);
            }
            else
                _ComponentDict.Add(key, new[] {
                    registration
             });
        }

        protected void RemoveComponent(TKey key)
        {
            _ComponentDict.Remove(key);
        }

        protected virtual Type GetComponent(TKey key, Layout.VxFormElementDefinition formColumnDefinition)
        {
            var found = _ComponentDict.TryGetValue(key, out IList<IVxComponentRegistration<TKey>> outVar);

            return found ? outVar.First().Component : _DefaultComponent;
        }

        public void Clear()
        {
            _ComponentDict.Clear();
        }

        public void RemoveComponent(object key)
        {
            RemoveComponent((TKey)key);
        }

        public Type GetComponent(object key, Layout.VxFormElementDefinition formColumnDefinition)
        {
            return GetComponent((TKey)key, formColumnDefinition);
        }

        protected int RegisterAllDiscoverableFormElements(Assembly[] assemblies, Func<TRegistrationAttribute, Type, IVxComponentRegistration<TKey>> converter)
        {
            var res = assemblies.AsParallel()
                 .SelectMany(c => c.GetTypes())
                 .Select(type => type.GetCustomAttributes(typeof(TRegistrationAttribute), true)
                     .Cast<TRegistrationAttribute>().Select(item => converter(item, type))
                     )
                 .Where(c => c.Any());
            var list = res.ToList();

            foreach (var item in list)
                foreach (var reg in item)
                    this.RegisterComponent(reg.SupportedDataType, reg);


            return list.Count;
        }

        public void RegisterComponent(object key, IVxComponentRegistration<TKey> registration)
        {
            RegisterComponent((TKey)key, registration);
        }
    }


}
