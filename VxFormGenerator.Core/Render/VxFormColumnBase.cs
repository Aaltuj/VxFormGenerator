using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Core.Render
{
    public class VxFormColumnBase : OwningComponentBase
    {
        [CascadingParameter] public VxFormLayoutOptions FormLayoutOptions { get; set; }

        public string CssClass
        {
            get
            {

                if (FormLayoutOptions.LabelOrientation == LabelOrientation.TOP && this.FormColumnDefinition.RenderOptions.ColSpan > 0)
                    return $"col-{this.FormColumnDefinition.RenderOptions.ColSpan}";
                else
                    return "col";
            }
        }

        public string Style
        {
            get
            {

                if (FormLayoutOptions.LabelOrientation == LabelOrientation.LEFT && FormColumnDefinition.RenderOptions.ColSpan > 0)
                {
                    var colspan = Math.Round(FormColumnDefinition.RenderOptions.ColSpan * (100.0 - 25.0) / 12.0);
                    string colspanS = colspan.ToString(CultureInfo.InvariantCulture);
                    return $"flex: 0 0 {colspanS}%; max-width: {colspanS}";
                }
                return "";
            }
        }
        [Parameter] public Layout.VxFormElementDefinition FormColumnDefinition { get; set; }

        public RenderFragment CreateFormElement() => builder =>
        {
            var propertyFormElement = FormColumnDefinition.Model.GetType().GetProperty(FormColumnDefinition.Name);
            // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
            MethodInfo method = typeof(VxFormColumnBase).GetMethod(nameof(VxFormColumnBase.CreateFormElementReferencePoco), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericMethod = method.MakeGenericMethod(propertyFormElement.PropertyType);
            // Execute the method with the following parameters
            genericMethod.Invoke(this, new object[] { FormColumnDefinition.Model, propertyFormElement, builder, FormColumnDefinition });
        };

        private void CreateFormElementReferencePoco<TValue>(object model, PropertyInfo propertyInfo,
            RenderTreeBuilder builder, Layout.VxFormElementDefinition formColumnDefinition)
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
                FormColumnDefinition = formColumnDefinition
            };

            var elementType = typeof(VxFormElementLoader<TValue>);

            builder.OpenComponent(0, elementType);
            builder.AddAttribute(1, nameof(VxFormElementLoader<TValue>.ValueReference), formElementReference);
            builder.CloseComponent();
        }

    }
}
