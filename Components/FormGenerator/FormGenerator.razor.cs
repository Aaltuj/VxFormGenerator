using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FormGeneratorDemo.Components.FormGenerator
{
    public class FormGeneratorComponent<TValue> : OwningComponentBase
    {
        [Parameter] public TValue DataContext { get; set; }

        [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }

        public System.Reflection.PropertyInfo[] Properties = new System.Reflection.PropertyInfo[] { };

        private FormGeneratorComponentsRepository _repo;

        public FormGeneratorComponent()
        {

        }
        protected override void OnInitialized()
        {
            _repo = ScopedServices.GetService(typeof(FormGeneratorComponentsRepository)) as FormGeneratorComponentsRepository;
        }

        public void HandleValidSubmit()
        {

        }

        protected override void OnParametersSet()
        {
            Properties = typeof(TValue).GetProperties();
        }

        public RenderFragment RenderFormElement(System.Reflection.PropertyInfo propInfoValue) => builder =>
        {
            builder.OpenComponent(0, _repo.FormElementComponent);

            builder.AddAttribute(1, nameof(FormElement.FieldIdentifier), propInfoValue);

            builder.CloseComponent();
        };

        public bool HasLabel(System.Reflection.PropertyInfo propInfoValue)
        {
            var componentType = _repo.GetComponent(propInfoValue.PropertyType.ToString());

            var dd = componentType
                    .GetCustomAttributes(typeof(DisplayAttribute), false)
                    .FirstOrDefault() as DisplayAttribute;

            return dd != null && dd.Name.Length > 0;
        }
    }
}