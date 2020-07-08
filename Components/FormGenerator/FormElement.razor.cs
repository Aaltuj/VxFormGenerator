using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class FormElementComponent : OwningComponentBase
    {
        private string _Label;
        private FormGeneratorComponentsRepository _repo;
        public string CssClass { get => string.Join(" ", CssClasses.ToArray()); }
        [Parameter] public List<string> CssClasses { get; set; }
        [Parameter] public List<string> DefaultFieldClasses { get; set; }

        [Parameter] public string Id { get; set; }

        [Parameter]
        public string Label
        {
            get
            {
                var dd = CascadedEditContext.Model
                     .GetType()
                     .GetProperty(FieldIdentifier.Name)
                     .GetCustomAttributes(typeof(DisplayAttribute), false)
                     .FirstOrDefault() as DisplayAttribute;

                return _Label ?? dd?.Name;
            }
            set { _Label = value; }
        }

        [Parameter] public PropertyInfo FieldIdentifier { get; set; }

        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        protected override void OnInitialized()
        {
            _repo = ScopedServices.GetService(typeof(FormGeneratorComponentsRepository)) as FormGeneratorComponentsRepository;
        }

        private static bool TypeImplementsInterface(Type type, Type typeToImplement)
        {
            Type foundInterface = type
                .GetInterfaces()
                .Where(i =>
                {
                    return i.Name == typeToImplement.Name;
                })
                .Select(i => i)
                .FirstOrDefault();

            return foundInterface != null;
        }


        public RenderFragment CreateComponent(System.Reflection.PropertyInfo propInfoValue) => builder =>
        {

            var componentType = _repo.GetComponent(propInfoValue.PropertyType.ToString());

            FormElementMover formElementInstanceMover = null;

            if (componentType == null)
                return;

            var elementType = componentType;

            // When the elementType that is rendered is a generic Set the propertyType as the generic type
            if (elementType.IsGenericTypeDefinition)
            {
                Type[] typeArgs = { propInfoValue.PropertyType };
                elementType = elementType.MakeGenericType(typeArgs);
            }

            // Activate the the Type so that the methods can be called
            var instance = Activator.CreateInstance(elementType);
            // wrap into a non-generic class because it's a parameter of invocated method. because it doesn't accept generic parameters
            formElementInstanceMover = new FormElementMover();

            // Check if the component has the IRenderChildren and read the type of component that should be rendered
            if (TypeImplementsInterface(componentType, typeof(IRenderChildren)))
            {
                // wrap into a non-generic class because it's a parameter of invocated method. because it doesn't accept generic parameters
                formElementInstanceMover.PayloadRenderChildren = instance as IRenderChildren;
            }

            // Check if the component has the IRenderChildren and read the type of component that should be rendered
            if (TypeImplementsInterface(componentType, typeof(IRenderCss)))
            {
                // wrap into a non-generic class because it's a parameter of invocated method. because it doesn't accept generic parameters
                formElementInstanceMover.PayloadCSS = instance as IRenderCss;

            }

            if (TypeImplementsInterface(componentType, typeof(IRenderAsFormElement)))
            {
                this.CssClasses.AddRange((instance as IRenderAsFormElement).FormElementClasses);
            }

            // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
            MethodInfo method = typeof(FormElementComponent).GetMethod(nameof(FormElementComponent.CreateFormComponent));
            MethodInfo genericMethod = method.MakeGenericMethod(propInfoValue.PropertyType, elementType);
            // Execute the method with the following parameters
            genericMethod.Invoke(this, new object[] { this, CascadedEditContext.Model, propInfoValue, builder, formElementInstanceMover });
        };

        public void CreateFormComponent<T, TElement>(object target,
            object dataContext,
            PropertyInfo propInfoValue, RenderTreeBuilder builder, FormElementMover formElementInstanceMover)
        {

            builder.OpenComponent(0, typeof(TElement));

            // Bind the value of the input base the the propery of the model instance
            var s = propInfoValue.GetValue(dataContext);
            builder.AddAttribute(1, nameof(InputBase<T>.Value), s);

            // Create the handler for ValueChanged. I use reflection to the value.
            builder.AddAttribute(3, nameof(InputBase<T>.ValueChanged),
                    Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<
                        Microsoft.AspNetCore.Components.EventCallback<T>>(
                        Microsoft.AspNetCore.Components.EventCallback.Factory.Create<T>(
                            target, Microsoft.AspNetCore.Components.EventCallback.Factory.
                            CreateInferred(target, __value => propInfoValue.SetValue(dataContext, __value),
                            (T)propInfoValue.GetValue(dataContext)))));

            // Create an expression to set the ValueExpression-attribute.
            var constant = System.Linq.Expressions.Expression.Constant(dataContext, dataContext.GetType());
            var exp = System.Linq.Expressions.MemberExpression.Property(constant, propInfoValue.Name);
            var lamb = System.Linq.Expressions.Expression.Lambda<Func<T>>(exp);
            builder.AddAttribute(4, nameof(InputBase<T>.ValueExpression), lamb);

            builder.AddAttribute(5, "class", GetDefaultFieldClasses(formElementInstanceMover));

            if (formElementInstanceMover.PayloadRenderChildren != null)
            {
                formElementInstanceMover.PayloadRenderChildren.RenderChildren(builder, 6, dataContext, propInfoValue);
            }

            builder.CloseComponent();

        }

        private string GetDefaultFieldClasses(FormElementMover formElementInstanceMover)
        {
            var output = DefaultFieldClasses == null ? "" : string.Join(" ", DefaultFieldClasses);
            var overrides = formElementInstanceMover.PayloadCSS == null ? "" : string.Join(" ", formElementInstanceMover.PayloadCSS.CssClasses);
            return string.Join(" ", output, overrides);

        }
    }
}