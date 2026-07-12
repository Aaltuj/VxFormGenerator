using Microsoft.AspNetCore.Components.Forms;

namespace VxFormGenerator.Settings.Bulma
{
    public class VxBulmaFormCssClassProvider : VxFormCssClassProviderBase
    {
        public override VxFormCssClassAttribute CssClassAttribute { get => new VxFormCssClassAttribute() { Valid = "is-success", Invalid = "is-danger" }; }

        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            return base.GetFieldCssClass(editContext, fieldIdentifier);
        }
    }
}
