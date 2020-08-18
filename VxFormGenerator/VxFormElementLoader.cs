using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VxFormGenerator.Components.Plain;

namespace VxFormGenerator
{
    public class VxFormElementLoader<TValue> : OwningComponentBase
    {
        private FormGeneratorComponentsRepository _repo;

        [Parameter] public FormElementReference<TValue> ValueReference { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // setup the repo containing the mappings
            _repo = ScopedServices.GetService(typeof(FormGeneratorComponentsRepository)) as FormGeneratorComponentsRepository;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);


            var elementType = _repo.FormElementComponent;

            // When the elementType that is rendered is a generic Set the propertyType as the generic type
            if (elementType.IsGenericTypeDefinition)
            {
                Type[] typeArgs = { typeof(TValue) };
                elementType = elementType.MakeGenericType(typeArgs);
            }

            builder.OpenComponent(0, elementType);

            // Bind the value of the input base the the propery of the model instance

            builder.AddAttribute(1, nameof(FormElement<TValue>.Value), ValueReference.Value);

            // Create the handler for ValueChanged. This wil update the model instance with the input
            builder.AddAttribute(2, nameof(FormElement<TValue>.ValueChanged), ValueReference.ValueChanged);

            if (ValueReference.ValueExpression == null)
            {
                // Create an expression to set the ValueExpression-attribute.
                var constant = Expression.Constant(ValueReference, ValueReference.GetType());
                var exp = Expression.Property(constant, nameof(ValueReference.Value));
                var lamb = Expression.Lambda<Func<TValue>>(exp);

                builder.AddAttribute(4, nameof(FormElement<TValue>.ValueExpression), lamb);
            }
            else
            {
                builder.AddAttribute(4, nameof(FormElement<TValue>.ValueExpression), ValueReference.ValueExpression);
            }

            builder.AddAttribute(5, nameof(FormElement<TValue>.FieldIdentifier), ValueReference.Key);

            builder.CloseComponent();

        }
    }
}
