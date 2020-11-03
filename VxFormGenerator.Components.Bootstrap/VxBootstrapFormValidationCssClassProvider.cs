using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Settings.Bootstrap
{
    public class VxBootstrapFormValidationCssClassProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var cssClassName = base.GetFieldCssClass(editContext, fieldIdentifier);

            // If we can find a [VxFormValidationCssClass], use it
            var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
            if (propertyInfo != null)
            {
                var customValidationClassName = (VxFormValidationCssClassAttribute)propertyInfo
                    .GetCustomAttributes(typeof(VxFormValidationCssClassAttribute), true)
                    .FirstOrDefault();
                if (customValidationClassName != null)
                {
                    cssClassName = string.Join(' ', cssClassName.Split(' ').Select(token => token switch
                    {
                        "valid" => customValidationClassName.Valid ?? token,
                        "invalid" => customValidationClassName.Invalid ?? token,
                        _ => token,
                    }));
                }
            }

            return cssClassName;
        }
    }
}
