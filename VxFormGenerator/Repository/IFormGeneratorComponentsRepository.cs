using System;

namespace VxFormGenerator.Core.Repository
{
    /// <summary>
    /// Non-generic interface for DI in the <see cref="FormElementBase{TFormElement}"/>
    /// </summary>
    public interface IFormGeneratorComponentsRepository
    {
        public void RegisterComponent(object key, Type component);
        public void RemoveComponent(object key);
        public Type GetComponent(object key);
    }

}
