using Microsoft.AspNetCore.Components.Forms;

namespace VxFormGenerator.Settings.Tailwind
{
    public class VxTailwindFormCssClassProvider : VxFormCssClassProviderBase
    {
        public override VxFormCssClassAttribute CssClassAttribute { get => new VxFormCssClassAttribute() { Valid = "border-green-500 ring-green-500/30", Invalid = "border-red-500 ring-red-500/30" }; }

        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            return base.GetFieldCssClass(editContext, fieldIdentifier);
        }
    }
}
