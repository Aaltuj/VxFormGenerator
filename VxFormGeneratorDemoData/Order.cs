using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Validation;
using VxFormGenerator.Models;


namespace VxFormGeneratorDemoData
{
    
    public class OrderViewModel
    {
        // Indicate that this property type should be rendered as a separate elements in the form and give it a label
        [VxFormGroup(Label = "Delivery")]
        // Use ValidateComplexType to valdidate a complex object
        [ValidateComplexType]
        public AddressViewModel Address { get; set; } = new AddressViewModel();

        // Indicate that this property type should be rendered as a separate elements in the form and give it a label
        [VxFormGroup(Label = "Invoice")]
        // Use ValidateComplexType to valdidate a complex object
        [ValidateComplexType]
        public AddressViewModel BillingAddress { get; set; } = new AddressViewModel();

        [Display(Name = "Send insured")]
        public bool Valid { get; set; } = true;

        [Display(Name = "What color box")]
        public VxColor Color { get; set; }
    }
}
