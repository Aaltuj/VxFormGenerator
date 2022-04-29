using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Layout;

namespace VxFormGeneratorDemoData
{
    // Add label to row 2
    [VxFormRowLayout(Id = 2, Label = "Address", Description = "This is an address")]
    public class AddressViewModel
    {
        [Display(Name = "Firstname", Description = "This is a first name")]
        // Add element to row 1 with automatic width based on number of items in a row
        [VxFormElementLayout(RowId = 1)]
        public string SurName { get; set; }
        // Add element to row 1 with automatic width based on number of items in a row and define a placeholder
        [VxFormElementLayout(RowId = 1, Placeholder = "Your Lastname")]
        [Display(Name = "Lastname", Description = "This is a last name")]
        public string LastName { get; set; }

        [Display(Name = "Street",Description = "This is a street")]
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
         VxFormElementLayout(Placeholder = "The country you live")]
        public string Country { get; set; }

        [Display(Name = "State")]
        [MinLength(5)]
        public string State { get; set; }

    }
}
