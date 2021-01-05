using System.ComponentModel.DataAnnotations;

namespace VxFormGenerator.Core.Layout
{
    public enum LabelOrientation
    {
        /// <summary>
        /// Show label on the left side of the form
        /// </summary>
        LEFT = 0,

        /// <summary>
        /// Show label on the top of the field element
        /// </summary>
        TOP = 1,
        /// <summary>
        /// Don't show label of the field element
        /// </summary>
        NONE = 2
    }

    public enum PlaceholderPolicy
    {

        /// <summary>
        /// No placeholders will be shown
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Only placeholders set explicit by <see cref="VxFormElementLayoutAttribute.Placeholder"/> will be shown
        /// </summary>
        EXPLICIT = 1,
        /// <summary>
        /// Placeholders set explicitly will be shown. 
        /// If no placeholder is specified it fall back to the <see cref="VxFormElementLayoutAttribute.Label"/> or <see cref="DisplayAttribute.Name"/>.
        /// </summary>
        EXPLICIT_LABEL_FALLBACK= 2,
        /// <summary>
        /// Only show placeholders for when the component thinks it's relevant.
        /// Placeholders will be shown for multiple formelements on one row.
        /// Placeholders set explicitly will be overridden by the <see cref="VxFormElementLayoutAttribute.Label"/> or <see cref="DisplayAttribute.Name"/>.
        /// </summary>       
        IMPLICIT = 3,
        /// <summary>
        /// Only show placeholders for when the component thinks it's relevant.
        /// Placeholders will be shown for all field elements
        /// Placeholders set explicitly will be overridden by the <see cref="VxFormElementLayoutAttribute.Label"/> or <see cref="DisplayAttribute.Name"/>.
        /// </summary>        
        IMPLICIT_LABEL_FALLBACK = 4
    }

    public enum VisualFeedbackValidationPolicy
    {
        /// <summary>
        /// Only show visual feedback when fields are invalid or valid
        /// </summary>
        VALID_AND_INVALID = 0,
        /// <summary>
        /// Only show visual feedback when fields are valid
        /// </summary>
        ONLY_VALID = 1,
        /// <summary>
        /// Only show visual feedback when fields are invalid
        /// </summary>
        ONLY_INVALID = 2
    }
}