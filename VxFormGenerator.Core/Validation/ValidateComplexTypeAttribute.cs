// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// ----- COPY ALERT -----
// Shameless copy of https://github.com/dotnet/aspnetcore/blob/5da314644ae882ff22fb43101d0c3d89a35c40e9/src/Components/WebAssembly/Validation/src/ObjectGraphDataAnnotationsValidator.cs
// Because of the preview nature and the needs for adding a full preview dependency for only this wasn't my preffered choose

using System;
using System.ComponentModel.DataAnnotations;

namespace VxFormGenerator.Core.Validation
{
    /// <summary>
    /// A <see cref="ValidationAttribute"/> that indicates that the property is a complex or collection type that further needs to be validated.
    /// <para>
    /// By default <see cref="Validator"/> does not recurse in to complex property types during validation.
    /// When used in conjunction with <see cref="ObjectGraphDataAnnotationsValidator"/>, this property allows the validation system to validate
    /// complex or collection type properties.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ValidateComplexTypeAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ObjectGraphDataAnnotationsValidator.TryValidateRecursive(value, validationContext);

            // Validation of the properties on the complex type are responsible for adding their own messages.
            // Therefore, we can always return success from here.
            return ValidationResult.Success;
        }
    }
}
