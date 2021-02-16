using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Core.Repository
{

    public class FormGeneratorComponentsRepository<TKey, TRegistrationAttribute> : IFormGeneratorComponentsRepository where TRegistrationAttribute : IVxComponentRegistration<TKey>
    {
        protected Dictionary<TKey, IList<VxDataTypeComponentRegistration>> _ComponentDict = new Dictionary<TKey, IList<VxDataTypeComponentRegistration>>();

        public Type _DefaultComponent { get; protected set; }

        public FormGeneratorComponentsRepository()
        {

        }

        public FormGeneratorComponentsRepository(Dictionary<TKey, IList<VxDataTypeComponentRegistration>> componentRegistrations)
        {
            _ComponentDict = componentRegistrations;
        }
        public FormGeneratorComponentsRepository(Dictionary<TKey, IList<VxDataTypeComponentRegistration>> componentRegistrations, Type defaultComponent)
        {
            _ComponentDict = componentRegistrations;
            _DefaultComponent = defaultComponent;
        }


        protected void RegisterComponent(TKey key, Type component)
        {
            var registration = new VxDataTypeComponentRegistration(typeof(TKey), component);
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
            var found = _ComponentDict.TryGetValue(key, out IList<VxDataTypeComponentRegistration> outVar);

            return found ? outVar.First().Component : _DefaultComponent;
        }

        public void Clear()
        {
            _ComponentDict.Clear();
        }

        public void RegisterComponent(object key, Type component)
        {
            RegisterComponent((TKey)key, component);
        }

        public void RemoveComponent(object key)
        {
            RemoveComponent((TKey)key);
        }

        public Type GetComponent(object key, Layout.VxFormElementDefinition formColumnDefinition)
        {
            return GetComponent((TKey)key, formColumnDefinition);
        }

        protected int RegisterAllDiscoverableFormElements(Assembly[] assemblies)
        {
            var res = assemblies.AsParallel()
                 .SelectMany(c => c.GetTypes())
                 .Select(type => type.GetCustomAttributes(typeof(TRegistrationAttribute), true)
                     .Cast<VxDataTypeComponentRegistration>().Select(item =>
                     {
                         item.Component = type;
                         return item;
                     })
                     )
                 .Where(c => c.Any());
            var list = res.ToList();


            return 0;
        }


    }


}
