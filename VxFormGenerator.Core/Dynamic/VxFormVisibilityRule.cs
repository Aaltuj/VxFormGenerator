using System;
using System.Globalization;

namespace VxFormGenerator.Core.Dynamic
{
    public sealed class VxFormVisibilityRule
    {
        public string SourceField { get; set; }
        public string EqualsValue { get; set; }
        public bool VisibleWhenMatched { get; set; } = true;

        public bool IsVisible(VxFormMetadataModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(SourceField))
            {
                return true;
            }

            model.Values.TryGetValue(SourceField, out var sourceValue);
            var actualValue = Convert.ToString(sourceValue, CultureInfo.InvariantCulture);
            var isMatch = string.Equals(actualValue, EqualsValue, StringComparison.OrdinalIgnoreCase);

            return VisibleWhenMatched ? isMatch : !isMatch;
        }
    }
}
