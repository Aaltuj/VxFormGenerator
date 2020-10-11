
namespace VxFormGenerator.Core.Validation
{
    public class VxValidationMessageComponent<TValue> : ValidationMessageBase<TValue>
    {
        public override string ValidClass { get; set; } = "valid-feedback";
        public override string InValidClass { get; set; } = "invalid-feedback";
    }
}
