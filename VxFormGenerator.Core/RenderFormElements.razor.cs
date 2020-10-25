using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VxFormGenerator.Core
{
    public class RenderFormElements : OwningComponentBase
    {
        /// <summary>
        /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
        /// </summary>
        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        /// <summary>
        /// Override the default render method, determining if the <see cref="EditContext.Model"/> 
        /// is a regular class or a dynamic <see cref="ExpandoObject"/>
        /// </summary>
        /// <param name="builder">Instance of the page builder</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            // Check the type of the model
            var modelType = CascadedEditContext.Model.GetType();
            
            if (modelType == typeof(ExpandoObject))
            {
                // Accesing a ExpandoObject requires to cast the model as a dictionary, so it's accesable by a key of type string
                var accessor = ((IDictionary<string, object>)CascadedEditContext.Model);

                foreach (var key in accessor.Keys)
                {
                    // get the value of the object
                    var value = accessor[key];

                    // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
                    MethodInfo method = typeof(RenderFormElements).GetMethod(nameof(RenderFormElements.CreateFormElementReferenceExpando), BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo genericMethod = method.MakeGenericMethod(value.GetType());
                    // Execute the method with the following parameters
                    genericMethod.Invoke(this, new object[] { accessor, key, builder });
                }
            }
            else // Assume it's a regular class, could be tighter scoped
            {
                // Look over all the properties in the class. 
                // TODO: Should have an option to be excluded from selection 
                foreach (var propertyInfo in modelType.GetProperties().Where(w=> w.GetCustomAttribute<VxIgnoreAttribute>() == null))
                {
                    // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
                    MethodInfo method = typeof(RenderFormElements).GetMethod(nameof(RenderFormElements.CreateFormElementReferencePoco), BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo genericMethod = method.MakeGenericMethod(propertyInfo.PropertyType);
                    // Execute the method with the following parameters
                    genericMethod.Invoke(this, new object[] { CascadedEditContext.Model, propertyInfo, builder });
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        /// <summary>
        /// Create a <see cref="VxFormElementLoader{TValue}"/> that will create a <see cref="FormElement"/>
        /// based on the dynamic <see cref="ExpandoObject"/>. This allows for dynamic usage of the form-generator.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <param name="builder"></param>
        private void CreateFormElementReferenceExpando<TValue>(ExpandoObject model, string key, RenderTreeBuilder builder)
        {
            // cast the model to a dictionary so it's accessable
            var accessor = ((IDictionary<string, object>)model);

            object value = default(TValue);
            accessor.TryGetValue(key, out value);

            var valueChanged = Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(
                        EventCallback.Factory.Create<TValue>(
                            this, EventCallback.Factory.
                            CreateInferred(this, __value => accessor[key] = __value,
                            (TValue)accessor[key])));

            var formElementReference = new FormElementReference<TValue>()
            {
                Value = (TValue)value,
                ValueChanged = valueChanged,
                Key = key
            };

            var elementType = typeof(VxFormElementLoader<TValue>);

            builder.OpenComponent(0, elementType);
            builder.AddAttribute(1, nameof(VxFormElementLoader<TValue>.ValueReference), formElementReference);
            builder.CloseComponent();
        }

        private void CreateFormElementReferencePoco<TValue>(object model, PropertyInfo propertyInfo, RenderTreeBuilder builder)
        {
            var valueChanged = Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(
                        EventCallback.Factory.Create<TValue>(
                            this, EventCallback.Factory.
                            CreateInferred(this, __value => propertyInfo.SetValue(model, __value),

                            (TValue)propertyInfo.GetValue(model))));
   // Create an expression to set the ValueExpression-attribute.
            var constant = Expression.Constant(model, model.GetType());
            var exp = Expression.Property(constant, propertyInfo.Name);
            var lamb = Expression.Lambda<Func<TValue>>(exp);

            var formElementReference = new FormElementReference<TValue>()
            {
                Value = (TValue)propertyInfo.GetValue(model),
                ValueChanged = valueChanged,
                ValueExpression = lamb,
                Key = propertyInfo.Name
            };
                     

            var elementType = typeof(VxFormElementLoader<TValue>);

            builder.OpenComponent(0, elementType);
            builder.AddAttribute(1, nameof(VxFormElementLoader<TValue>.ValueReference), formElementReference);
            builder.CloseComponent();
        }
    }
}
