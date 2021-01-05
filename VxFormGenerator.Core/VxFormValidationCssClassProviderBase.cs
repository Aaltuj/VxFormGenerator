using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Settings
{
    public abstract class VxFormCssClassProviderBase : FieldCssClassProvider
    {
        public abstract VxFormCssClassAttribute CssClassAttribute { get; }
        public VxFormLayoutOptions FormLayoutOptions { get; set; }

        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var cssClassName = base.GetFieldCssClass(editContext, fieldIdentifier);


            // If we can find a [VxFormValidationCssClass], use it
            var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
            if (propertyInfo != null && editContext.IsModified(fieldIdentifier))
            {
                var customValidationClassName = (VxFormCssClassAttribute)propertyInfo
                    .GetCustomAttributes(typeof(VxFormCssClassAttribute), true)
                    .FirstOrDefault();

                if (customValidationClassName == null && CssClassAttribute != null)
                    customValidationClassName = CssClassAttribute;

                if (FormLayoutOptions.VisualValidationPolicy == VisualFeedbackValidationPolicy.VALID_AND_INVALID)
                {
                    cssClassName = string.Join(' ', cssClassName.Split(' ').Select(token => token switch
                    {
                        "valid" => customValidationClassName.Valid ?? token,
                        "invalid" => customValidationClassName.Invalid ?? token,
                        _ => token,
                    }));
                }
                else if (FormLayoutOptions.VisualValidationPolicy == VisualFeedbackValidationPolicy.ONLY_INVALID)
                {
                    cssClassName = string.Join(' ', cssClassName.Split(' ').Select(token => token switch
                    {
                        "valid" => "",
                        "invalid" => customValidationClassName.Invalid ?? token,
                        _ => token,
                    }));
                }
                else if (FormLayoutOptions.VisualValidationPolicy == VisualFeedbackValidationPolicy.ONLY_VALID)
                {
                    cssClassName = string.Join(' ', cssClassName.Split(' ').Select(token => token switch
                    {
                        "valid" => customValidationClassName.Valid ?? token,
                        "invalid" => "",
                        _ => token,
                    }));
                }

            }

            return cssClassName;
        }
    }
}
