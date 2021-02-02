using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VxFormGenerator.Core.Attributes;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Render;
using VxFormGenerator.Core.Repository;

namespace VxFormGenerator.Core
{
    public class FormElementBase<TFormElement> : OwningComponentBase
    {

        [Inject]
        protected IFormGeneratorComponentsRepository Repo { get; set; }
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
        /// Updates the property with the new value
        /// </summary>
        [Parameter] public EventCallback<TFormElement> ValueChanged { get; set; }
        /// <summary>
        /// Get the property that is bound
        /// </summary>
        [Parameter] public Expression<Func<TFormElement>> ValueExpression { get; set; }
        /// <summary>
        /// The current Value of the <see cref="FormElementBase{TFormElement}"/>
        /// </summary>
        [Parameter] public TFormElement Value { get; set; }
        [Parameter] public Layout.VxFormElementDefinition FormColumnDefinition { get; set; }

        /// <summary>
        /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
        /// </summary>
        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        [CascadingParameter] public VxFormLayoutOptions FormLayoutOptions { get; set; }


        protected override void OnInitialized()
        {

        }



        /// <summary>
        /// A method that renders the form control based on the <see cref="FormElement.FieldIdentifier"/>
        /// </summary>
        /// <param name="propInfoValue"></param>
        /// <returns></returns>
        public RenderFragment CreateComponent() => builder =>
        {
            // Get the mapped control based on the property type
            var componentType = Repo.GetComponent(typeof(TFormElement), FormColumnDefinition);

            // TODO: add the dynamic version for getting a component


            if (componentType == null)
                return;
            //  throw new Exception($"No component found for: {propInfoValue.PropertyType.ToString()}");

            // Set the found component
            var elementType = componentType;

            // When the elementType that is rendered is a generic Set the propertyType as the generic type
            if (elementType.IsGenericTypeDefinition)
            {
                Type[] typeArgs = { typeof(TFormElement) };
                elementType = elementType.MakeGenericType(typeArgs);
            }

            /*   // Activate the the Type so that the methods can be called
               var instance = Activator.CreateInstance(elementType);*/

            this.CreateFormComponent(this, FormColumnDefinition.Model, FormColumnDefinition.Name, builder, elementType);
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
            string fieldIdentifier, RenderTreeBuilder builder, Type elementType)
        {


            builder.OpenRegion(100);
            // Create the component based on the mapped Element Type
            builder.OpenComponent(0, elementType);

            // Bind the value of the input base the the propery of the model instance
            builder.AddAttribute(1, nameof(InputBase<TFormElement>.Value), Value);

            // Create the handler for ValueChanged. This wil update the model instance with the input
            builder.AddAttribute(2, nameof(InputBase<TFormElement>.ValueChanged), ValueChanged);

            builder.AddAttribute(3, nameof(InputBase<TFormElement>.ValueExpression), ValueExpression);

            if (FormColumnDefinition.RenderOptions.Placeholder != null)
                builder.AddAttribute(4, "placeholder", FormColumnDefinition.RenderOptions.Placeholder);

            // Set the class for the the formelement.
            builder.AddAttribute(5, "class", GetDefaultFieldClasses(Activator.CreateInstance(elementType) as InputBase<TFormElement>));

            CheckForInterfaceActions(this, FormColumnDefinition.Model, fieldIdentifier, builder, 6, elementType);

            builder.CloseComponent();

            builder.CloseRegion();

        }

        private void CheckForInterfaceActions(object target,
            object dataContext,
            string fieldIdentifier, RenderTreeBuilder builder, int indexBuilder, Type elementType)
        {

            // Check if the component has the IRenderChildren and renderen them in the form control
            if (VxHelpers.TypeImplementsInterface(elementType, typeof(IRenderChildrenVxLookupValueKey)))
            {/*
                var method = elementType
                    .GetMethod(nameof(IRenderChildrenVxLookupValueKey.RenderLookupKeyValueChildren)
                    , BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static);*/

                var prop = dataContext.GetType().GetProperty(fieldIdentifier);

                if (prop.PropertyType.IsEnum)
                {
                    builder.AddAttribute(indexBuilder++, nameof(IRenderChildrenVxLookupValueKey.KeyValueLookup), new VxEnumLookup() { Enum = prop.PropertyType });
                }
                else
                {
                    var attribute = prop.GetCustomAttribute(typeof(VxLookupAttribute), true) as VxLookupAttribute;

                    if (attribute == null)
                        return;


                    var resolver = attribute.GetResolver().LookupKeyValue;

                    if (resolver == null)
                        throw new Exception("Check lookup, this shouldn't be empty");

                    builder.AddAttribute(indexBuilder++, nameof(IRenderChildrenVxLookupValueKey.KeyValueLookup), resolver);
                }



                /* method.Invoke(null, new object[] { builder, indexBuilder
                     , dataContext
                     , fieldIdentifier
                     , resolver          });*/
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



    }
}