using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VxFormGenerator.Core.Layout;

namespace VxFormGeneratorDemoData
{
    public class VxFormLayoutOptionsAnnotated
    {
        [VxFormElementLayout(Label = "Complexity of the form")]
        public TypeOfForm FormRenderKind { get; set; } = TypeOfForm.ADVANCED;
        [VxFormElementLayout(Label = "Label Orientation")]
        public LabelOrientationAnnotated LabelOrientation { get; set; }
        [VxFormElementLayout(Label = "Placeholder Policy")]
        public PlaceholderPolicyAnnotated ShowPlaceholder { get; set; }
        [VxFormElementLayout(Label = "Validation Mode")]
        public VisualFeedbackValidationPolicyAnnotated VisualValidationPolicy { get; set; }
      

        public VxFormLayoutOptions ToFormLayoutOptions()
        {
            return new VxFormLayoutOptions()
            {
                ShowPlaceholder = (PlaceholderPolicy)this.ShowPlaceholder,
                LabelOrientation = (LabelOrientation)this.LabelOrientation,
                VisualValidationPolicy = (VisualFeedbackValidationPolicy)this.VisualValidationPolicy
            };
        }
    }

    public enum TypeOfForm
    {
        [Display(Name = "Simple datastructure")]
        SIMPLE = 0,
        [Display(Name = "Advanced datastructure")]
        ADVANCED = 1,
        [Display(Name = "Complex datastructure")]
        COMPLEX = 2
    }

    public enum LabelOrientationAnnotated
    {
        /// <summary>
        /// Show label on the left side of the form
        /// </summary>
        [Display(Name = "Left")]
        LEFT = 0,

        /// <summary>
        /// Show label on the top of the field element
        /// </summary>
        [Display(Name = "Top")]
        TOP = 1,
        /// <summary>
        /// Don't show label of the field element
        /// </summary>
        [Display(Name = "None")]
        NONE = 2
    }

    public enum PlaceholderPolicyAnnotated
    {

        /// <summary>
        /// No placeholders will be shown
        /// </summary>
        [Display(Name = "None", Description = "Don't show placeholders")]
        NONE = 0,
        /// <summary>
        /// Only placeholders set explicit by <see cref="VxFormElementLayoutAttribute.Placeholder"/> will be shown
        /// </summary>
        [Display(Name = "Explicit", Description = "Only show placeholders set explicitly")]
        EXPLICIT = 1,
        /// <summary>
        /// Placeholders set explicitly will be shown. 
        /// If no placeholder is specified it fall back to the <see cref="VxFormElementLayoutAttribute.Label"/> or <see cref="DisplayAttribute.Name"/>.
        /// </summary>
        [Display(Name = "Explicit or use label", Description = "Placeholders set explicitly will be shown. If no placeholder is specified it fall back to the label.")]
        EXPLICIT_LABEL_FALLBACK = 2,
        /// <summary>
        /// Only show placeholders for when the component thinks it's relevant.
        /// Placeholders will be shown for multiple formelements on one row.
        /// Placeholders set explicitly will be overridden by the <see cref="VxFormElementLayoutAttribute.Label"/> or <see cref="DisplayAttribute.Name"/>.
        /// </summary>
        [Display(Name = "Implicit", Description = "Only show placeholders for when the component thinks it's relevant. Placeholders will be shown for multiple formelements on one row.")]
        IMPLICIT = 3,
        /// <summary>
        /// Only show placeholders for when the component thinks it's relevant.
        /// Placeholders will be shown for all field elements
        /// Placeholders set explicitly will be overridden by the <see cref="VxFormElementLayoutAttribute.Label"/> or <see cref="DisplayAttribute.Name"/>.
        /// </summary>
        [Display(Name = "Implicit or use label", Description = "Only show placeholders for when the component thinks it's relevant. Placeholders will show for all fields and uses the label.")]
        IMPLICIT_LABEL_FALLBACK = 4
    }

    public enum VisualFeedbackValidationPolicyAnnotated
    {
        /// <summary>
        /// Only show visual feedback when fields are invalid or valid
        /// </summary>
        [Display(Name = "When valid or invalid", Description = "Show visual feedback when fields are invalid of valid")]
        VALID_AND_INVALID = 0,
        /// <summary>
        /// Only show visual feedback when fields are valid
        /// </summary>
        [Display(Name = "Only when valid", Description = "Only show visual feedback when fields are valid")]
        ONLY_VALID = 1,
        /// <summary>
        /// Only show visual feedback when fields are invalid
        /// </summary>
        [Display(Name = "Only when invalid", Description = "Only show visual feedback when fields are invalid")]
        ONLY_INVALID = 2
    }
}
