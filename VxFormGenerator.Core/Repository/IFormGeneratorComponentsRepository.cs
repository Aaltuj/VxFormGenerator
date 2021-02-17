using System;
using VxFormGenerator.Core.Repository.Registration;

namespace VxFormGenerator.Core.Repository
{
    /// <summary>
    /// Non-generic interface for DI in the <see cref="FormElementBase{TFormElement}"/>
    /// </summary>
    public interface IFormGeneratorComponentsRepository
    {
      
        public Type GetComponent(object key, Layout.VxFormElementDefinition formColumnDefinition);
    }

    public interface IChangeFormGeneratorComponentsRepository<TKey>
    {
        public void RegisterComponent(object key, IVxComponentRegistration<TKey> registration);
        public void RemoveComponent(object key);
    }

}
