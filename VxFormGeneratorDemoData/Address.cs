using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Layout;
using VxFormGenerator.Core.Attributes;

namespace VxFormGeneratorDemoData
{
    // Add label to row 2
    [VxFormRowLayout(Id = 2, Label = "Adress")]
    public class AddressViewModel
    {
        [Display(Name = "Firstname")]
        // Add element to row 1 with automatic width based on number of items in a row
        [VxFormElementLayout(RowId = 1)]
        public string SurName { get; set; }
        // Add element to row 1 with automatic width based on number of items in a row and define a placeholder
        [VxFormElementLayout(RowId = 1, Placeholder = "Your Lastname")]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "Street")]
        // Add element to row 2 and set the width to 9 of 12 columns
        [VxFormElementLayout(RowId = 2, ColSpan = 9)]
        [MinLength(5)]
        public string Street { get; set; }

        [Display(Name = "Number")]
        // Add element to row 2 and set the width to 3 of 12 columns
        [VxFormElementLayout(RowId = 2, ColSpan = 3)]
        public string Number { get; set; }

        [Display(Name = "Country"),
         // Show Placeholder
         VxFormElementLayout(Placeholder = "The country you live"),
            VxLookup(typeof(CountryResolver))]
        public string Country { get; set; }

        [Display(Name = "State")]
        [MinLength(5)]
        public string State { get; set; }

    }
}
