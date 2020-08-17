using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VxFormGenerator
{
    public class RenderFormElements : OwningComponentBase
    {
        /// <summary>
        /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
        /// </summary>
        [CascadingParameter] EditContext CascadedEditContext { get; set; }

        //        public Dictionary<string, FormElementReference<string>> Properties { get; set; } = new Dictionary<string, FormElementReference<string>>();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var modelType = CascadedEditContext.Model.GetType();
            
            if (modelType == typeof(ExpandoObject))
            {
                var accessor = ((IDictionary<string, object>)CascadedEditContext.Model);
                foreach (var key in accessor.Keys)
                {
                    var value = accessor[key];

                    // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
                    MethodInfo method = typeof(RenderFormElements).GetMethod(nameof(RenderFormElements.CreateFormElementReferenceExpando), BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo genericMethod = method.MakeGenericMethod(value.GetType());
                    // Execute the method with the following parameters
                    genericMethod.Invoke(this, new object[] { accessor, key, builder });
                }
            }
            else
            {
                foreach (var propertyInfo in modelType.GetProperties())
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

        private void CreateFormElementReferenceExpando<TValue>(ExpandoObject model, string key, RenderTreeBuilder builder)
        {
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
