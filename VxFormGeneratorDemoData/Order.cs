using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Validation;
using VxFormGeneratorDemoData;

namespace VxFormGeneratorDemoData
{
    [VxFormDefinition]
    public class OrderViewModel
    {
        [VxFormGroup(Label = "Delivery")]
        [ValidateComplexType]
        public AddressViewModel Address { get; set; } = new AddressViewModel();

        [VxFormGroup(Label = "Invoice")]
        [ValidateComplexType]
        public AddressViewModel BillingAddress { get; set; } = new AddressViewModel();

        [Display(Name = "State")]
        public FoodKind State { get; set; }
    }
}
