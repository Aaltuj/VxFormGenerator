using System;
using System.ComponentModel.DataAnnotations;

namespace VxFormGenerator.Core.Attributes
{
    /// <summary>
    /// A <see cref="ValidationAttribute"/> that compares two properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class VxComparePropertyAttribute : CompareAttribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ComparePropertyAttribute"/>.
        /// </summary>
        /// <param name="otherProperty">The property to compare with the current property.</param>
        public VxComparePropertyAttribute(string otherProperty)
            : base(otherProperty)
        {
        }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = base.IsValid(value, validationContext);
            if (validationResult == ValidationResult.Success)
            {
                return validationResult;
            }

            return new ValidationResult(validationResult.ErrorMessage, new[] { validationContext.MemberName });
        }
    }
}
