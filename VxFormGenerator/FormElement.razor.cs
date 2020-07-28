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
using VxFormGenerator.Validation;

namespace VxFormGenerator
{
    public class FormElementComponent<TFormElement> : OwningComponentBase
    {
        private string _Label;
        private FormGeneratorComponentsRepository _repo;
        /// <summary>
        /// Bindable property to set the class
        /// </summary>
        public string CssClass { get => string.Join(" ", CssClasses.ToArray()); }
        /// <summary>
        /// Setter for the classes of the form container
        /// </summary>
        [Parameter] public List<string> CssClasses { get; set; }

        /// <summary>
        /// Will set the 'class' of the all the controls. Useful when a framework needs to implement a class for all form elements
        /// </summary>
        [Parameter] public List<string> DefaultFieldClasses { get; set; }
        /// <summary>
        /// The identifier for the <see cref="FormElement"/>"/> used by the label element
        /// </summary>
        [Parameter] public string Id { get; set; }

        /// <summary>
        /// The label for the <see cref="FormElement"/>, if not set, it will check for a <see cref="DisplayAttribute"/> on the <see cref="CascadedEditContext.Model"/>
        /// </summary>
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

        /// <summary>
        /// The property that should generate a formcontrol
        /// </summary>
        [Parameter] public PropertyInfo FieldIdentifier { get; set; }

        /// <summary>
        /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
        /// </summary>
        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        protected override void OnInitialized()
        {
            // setup the repo containing the mappings
            _repo = ScopedServices.GetService(typeof(FormGeneratorComponentsRepository)) as FormGeneratorComponentsRepository;
        }

        public Expression<Func<TFormElement>> FieldExpression { get; set; }

        /// <summary>
        /// A method thar renders the form control based on the <see cref="FormElement.FieldIdentifier"/>
        /// </summary>
        /// <param name="propInfoValue"></param>
        /// <returns></returns>
        public RenderFragment CreateComponent(System.Reflection.PropertyInfo propInfoValue) => builder =>
        {
            // Get the mapped control based on the property type
            var componentType = _repo.GetComponent(propInfoValue.PropertyType.ToString());


            if (componentType == null)
                return;
            //  throw new Exception($"No component found for: {propInfoValue.PropertyType.ToString()}");

            // Set the found component
            var elementType = componentType;

            // When the elementType that is rendered is a generic Set the propertyType as the generic type
            if (elementType.IsGenericTypeDefinition)
            {
                Type[] typeArgs = { propInfoValue.PropertyType };
                elementType = elementType.MakeGenericType(typeArgs);
            }

            /*   // Activate the the Type so that the methods can be called
               var instance = Activator.CreateInstance(elementType);*/

            this.CreateFormComponent(this, CascadedEditContext.Model, propInfoValue, builder, elementType);
        };

        /// <summary>
        /// Creates the component that is rendered in the form
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <typeparam name="TElement">The type of the form element, should be based on <see cref="InputBase{TValue}"/>, like a <see cref="InputText"/></typeparam>
        /// <param name="target">This <see cref="FormElement"/> </param>
        /// <param name="dataContext">The Model instance</param>
        /// <param name="propInfoValue">The property that is being rendered</param>
        /// <param name="builder">The render tree of this element</param>
        /// <param name="instance">THe control instance</param>
        public void CreateFormComponent(object target,
            object dataContext,
            PropertyInfo propInfoValue, RenderTreeBuilder builder, Type elementType)
        {
            // Create the component based on the mapped Element Type
            builder.OpenComponent(0, elementType);

            // Bind the value of the input base the the propery of the model instance
            var s = propInfoValue.GetValue(dataContext);
            builder.AddAttribute(1, nameof(InputBase<TFormElement>.Value), s);

            // Create the handler for ValueChanged. This wil update the model instance with the input
            builder.AddAttribute(2, nameof(InputBase<TFormElement>.ValueChanged),
                    Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(
                        EventCallback.Factory.Create<TFormElement>(
                            target, EventCallback.Factory.
                            CreateInferred(target, __value => propInfoValue.SetValue(dataContext, __value),
                            (TFormElement)propInfoValue.GetValue(dataContext)))));

            // Create an expression to set the ValueExpression-attribute.
            var constant = Expression.Constant(dataContext, dataContext.GetType());
            var exp = Expression.Property(constant, propInfoValue.Name);
            var lamb = Expression.Lambda<Func<TFormElement>>(exp);

            FieldExpression = lamb;

            builder.AddAttribute(4, nameof(InputBase<TFormElement>.ValueExpression), lamb);

            // Set the class for the the formelement.
            // builder.AddAttribute(5, "class", GetDefaultFieldClasses(instance));

            CheckForInterfaceActions(this, CascadedEditContext.Model, propInfoValue, builder, 6, elementType);

            builder.CloseComponent();

            /*  builder.OpenComponent(8, typeof(VxValidationMessage<T>));
              builder.AddAttribute(9, nameof(VxValidationMessage<T>.For), FieldExpression as Expression<Func<T>>);
              builder.AddAttribute(9, nameof(VxValidationMessage<T>.Class), "poep");
              builder.CloseComponent();
  */
        }

        private void CheckForInterfaceActions(object target,
            object dataContext,
            PropertyInfo propInfoValue, RenderTreeBuilder builder, int indexBuilder, Type elementType)
        {
            // overriding the default classes for FormElement
            if (TypeImplementsInterface(elementType, typeof(IRenderAsFormElement)))
            {
                //this.CssClasses.AddRange((instance as IRenderAsFormElement).FormElementClasses);
            }

            // Check if the component has the IRenderChildren and renderen them in the form control
            if (TypeImplementsInterface(elementType, typeof(IRenderChildren)))
            {
                var method = elementType.GetMethod(nameof(IRenderChildren.RenderChildren), BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static);

                method.Invoke(null, new object[] { builder, indexBuilder, dataContext, propInfoValue });
            }
        }

        /// <summary>
        /// Merges the default control classes with the <see cref="InputBase{TValue}.AdditionalAttributes"/> 'class' key
        /// </summary>
        /// <typeparam name="T">The property type of the formelement</typeparam>
        /// <param name="instance">The instance of the component representing the form control</param>
        /// <returns></returns>
        private string GetDefaultFieldClasses<T>(InputBase<T> instance)
        {

            var output = DefaultFieldClasses == null ? "" : string.Join(" ", DefaultFieldClasses);

            if (instance == null)
                return output;

            var AdditionalAttributes = instance.AdditionalAttributes;

            if (AdditionalAttributes != null &&
                  AdditionalAttributes.TryGetValue("class", out var @class) &&
                  !string.IsNullOrEmpty(Convert.ToString(@class)))
            {
                return $"{@class} {output}";
            }

            return output;
        }

        private bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
        {
            if (typeToCheck == typeof(object))
            {
                return false;
            }
            else if (typeToCheck == null)
            {
                return false;
            }
            else if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            else
            {
                return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
            }
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

    }
}