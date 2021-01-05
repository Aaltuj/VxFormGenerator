using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VxFormGenerator.Core.Layout
{
    public class VxFormLayoutOptions
    {
        public LabelOrientation LabelOrientation { get; set; } = LabelOrientation.LEFT;

        public PlaceholderPolicy ShowPlaceholder { get; set; } = PlaceholderPolicy.EXPLICIT;

        public VisualFeedbackValidationPolicy VisualValidationPolicy { get; set; } = VisualFeedbackValidationPolicy.ONLY_INVALID;
    }
}
