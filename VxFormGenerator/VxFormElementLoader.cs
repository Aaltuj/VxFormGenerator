using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq.Expressions;


namespace VxFormGenerator.Core
{
    /// <summary>
    /// The loader has the task to create a <see cref="FormElement"/> with the correct bind-Value.
    /// It searches for a match in the <see cref="FormGeneratorComponentsRepository"/> and will render the mapped component.
    /// </summary>
    /// <typeparam name="TValue">The type of the property</typeparam>
    public class VxFormElementLoader<TValue> : OwningComponentBase
    {
        [Inject]
        private IFormGeneratorOptions Options { get; set; }


        /// <summary>
        /// Contains the Value binding methods and the key of the property.
        /// </summary>
        [Parameter] public FormElementReference<TValue> ValueReference { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

        }

        /// <summary>
        /// Create the <see cref="FormElement"/> that is associated with the <see cref="TValue"/>
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            // Get the registered FormElement component. 
            var elementType = Options.FormElementComponent;

            // When the elementType that is rendered is a generic Set the propertyType as the generic type
            if (elementType.IsGenericTypeDefinition)
            {
                Type[] typeArgs = { typeof(TValue) };
                elementType = elementType.MakeGenericType(typeArgs);
            }

            builder.OpenComponent(0, elementType);

            // Bind the value of the input base the the propery of the model instance
            builder.AddAttribute(1, nameof(FormElementBase<TValue>.Value), ValueReference.Value);

            // Create the handler for ValueChanged. This wil update the model instance with the input
            builder.AddAttribute(2, nameof(FormElementBase<TValue>.ValueChanged), ValueReference.ValueChanged);

            // if no explicit value expression create one based on the ValueReference
            if (ValueReference.ValueExpression == null)
            {
                // Create an expression to set the ValueExpression-attribute.
                var constant = Expression.Constant(ValueReference, ValueReference.GetType());
                var exp = Expression.Property(constant, nameof(ValueReference.Value));
                var lamb = Expression.Lambda<Func<TValue>>(exp);

                builder.AddAttribute(4, nameof(FormElementBase<TValue>.ValueExpression), lamb);
            }
            else
            {
                builder.AddAttribute(4, nameof(FormElementBase<TValue>.ValueExpression), ValueReference.ValueExpression);
            }

            // Set the property name so the element is aware of the property that it represents
            // and is able to trace back to the model
            builder.AddAttribute(5, nameof(FormElementBase<TValue>.FieldIdentifier), ValueReference.Key);

            builder.CloseComponent();

        }
    }
}
