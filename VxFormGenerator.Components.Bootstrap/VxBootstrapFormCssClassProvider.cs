using Microsoft.AspNetCore.Components.Forms;

namespace VxFormGenerator.Settings.Bootstrap
{
    public class VxBootstrapFormCssClassProvider : VxFormCssClassProviderBase
    {
        public override VxFormCssClassAttribute CssClassAttribute { get => new VxFormCssClassAttribute() { Valid = "is-valid", Invalid = "is-invalid" }; }

        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            return base.GetFieldCssClass(editContext, fieldIdentifier);
        }
    }
}
