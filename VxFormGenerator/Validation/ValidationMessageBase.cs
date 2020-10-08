using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VxFormGenerator.Core.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class ValidationMessageBase<TValue> : ComponentBase, IDisposable
    {
        public abstract string ValidClass { get; set; }
        public abstract string InValidClass { get; set; }

        private FieldIdentifier _fieldIdentifier;
        private string @class;

        [CascadingParameter] private EditContext EditContext { get; set; }
        [Parameter] public Expression<Func<TValue>> For { get; set; }
        [Parameter] public string Class { get => @class + " " + ValidationClass; set => @class = value; }

        protected IEnumerable<string> ValidationMessages => EditContext.GetValidationMessages(_fieldIdentifier);

        private string ValidationClass { get; set; }

        protected override void OnInitialized()
        {

            _fieldIdentifier = FieldIdentifier.Create(For);
            EditContext.OnValidationStateChanged += HandleValidationStateChanged;

        }

        private void HandleValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
        {
            CheckFieldState();
            StateHasChanged();
        }


        private void CheckFieldState()
        {
            var isValid = !EditContext.GetValidationMessages(_fieldIdentifier).Any();
            if (EditContext.IsModified(_fieldIdentifier))
            {
                ValidationClass = isValid ? ValidClass : InValidClass;
            }
            else
            {
                ValidationClass = "";
            }
        }


        public void Dispose()
        {
            EditContext.OnValidationStateChanged -= HandleValidationStateChanged;
        }

    }
}
