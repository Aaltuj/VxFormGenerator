using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace VxFormGenerator.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ValidationMessageBase<TValue> : ComponentBase, IDisposable
    {
        private FieldIdentifier _fieldIdentifier;

        [CascadingParameter] private EditContext EditContext { get; set; }
        [Parameter] public Expression<Func<TValue>> For { get; set; }
        [Parameter] public string Class { get; set; }

        protected IEnumerable<string> ValidationMessages => EditContext.GetValidationMessages(_fieldIdentifier);

        protected override void OnInitialized()
        {

            _fieldIdentifier = FieldIdentifier.Create(For);
            EditContext.OnValidationStateChanged += HandleValidationStateChanged;
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;
            EditContext.OnValidationRequested += EditContext_OnValidationRequested;
        }

        private void HandleValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
        {
            StateHasChanged();
        }

        private void EditContext_OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
         
        }

        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            
        }

        

        public void Dispose()
        {
            EditContext.OnValidationStateChanged -= HandleValidationStateChanged;
        }
       
    }
}
