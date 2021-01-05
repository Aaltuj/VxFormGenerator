using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core.Layout;


namespace VxFormGeneratorDemoData
{
    [VxFormRowLayout(Id = 2, Label = "Adress")]
    public class AddressViewModel
    {
        [Display(Name = "Firstname")]
        [VxFormElementLayout(RowId = 1)]
        public string SurName { get; set; }
        [VxFormElementLayout(RowId = 1, Placeholder = "Your Lastname")]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }


        [Display(Name = "Street")]
        [VxFormElementLayout(RowId = 2, ColSpan = 10)]
        [MinLength(5)]
        public string Street { get; set; }

        [Display(Name = "Number")]
        [VxFormElementLayout(RowId = 2, ColSpan = 2)]
        public string Number { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "State")]
        [MinLength(5)]
        public string State { get; set; }

        [Display(Name = "Is valid")]
        public bool Valid { get; set; } = true;

    }
}
