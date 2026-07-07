# Visual Studio Field Template Workflow

Issue #29 asked for a Visual Studio-friendly way to start from an annotated POCO and get editable Razor markup.

VxFormGenerator does not generate a physical `.razor` file from the IDE. Instead, use `FieldTemplate` to create a normal Razor component that you can edit in Visual Studio while the library still generates the field input, binding, and validation message.

## Workflow

1. Add annotations to the POCO model.
2. In Visual Studio, right-click the target folder and choose `Add` > `Razor Component`.
3. Name the component, for example `CustomerForm.razor`.
4. Paste the `EditForm` and `RenderFormElements` template below.
5. Customize the HTML and CSS around `field.Input` and `field.ValidationMessage`.

## Example Model

```csharp
public class CustomerFormModel
{
    [Display(Name = "First name")]
    [Required]
    [VxFormElementLayout(RowId = 1, ColSpan = 6, Placeholder = "Your first name")]
    public string FirstName { get; set; }

    [Display(Name = "Last name")]
    [Required]
    [VxFormElementLayout(RowId = 1, ColSpan = 6, Placeholder = "Your last name")]
    public string LastName { get; set; }

    [Display(Name = "Email")]
    [EmailAddress]
    [VxFormElementLayout(RowId = 2, ColSpan = 12, Placeholder = "name@example.com")]
    public string Email { get; set; }
}
```

## Visual Studio Razor Component

Create `CustomerForm.razor`:

```razor
@using Microsoft.AspNetCore.Components.Forms
@using VxFormGenerator.Core
@using VxFormGenerator.Core.Layout

<EditForm Model="Model" OnValidSubmit="HandleValidSubmit">
    <ObjectGraphDataAnnotationsValidator />

    <RenderFormElements FormLayoutOptions="Options">
        <FieldTemplate Context="field">
            <div class="vs-field-card" data-field-name="@field.Name">
                <div class="vs-field-header">
                    @if (field.ShowLabel)
                    {
                        <label for="@field.Id">@field.Label</label>
                    }

                    <span class="vs-field-name">@field.Name</span>
                </div>

                <div class="vs-field-input">
                    @field.Input
                </div>

                <div class="vs-field-validation">
                    @field.ValidationMessage
                </div>
            </div>
        </FieldTemplate>
    </RenderFormElements>

    <button class="btn btn-primary" type="submit">Save</button>
</EditForm>

@code {
    [Parameter]
    public CustomerFormModel Model { get; set; } = new CustomerFormModel();

    [Parameter]
    public EventCallback<CustomerFormModel> ValidSubmit { get; set; }

    private VxFormLayoutOptions Options { get; } = new VxFormLayoutOptions
    {
        LabelOrientation = LabelOrientation.TOP,
        ShowPlaceholder = PlaceholderPolicy.EXPLICIT_LABEL_FALLBACK
    };

    private Task HandleValidSubmit()
    {
        return ValidSubmit.InvokeAsync(Model);
    }
}
```

Add CSS wherever your app keeps component styles:

```css
.vs-field-card {
    border: 1px solid #d0d7de;
    border-radius: 0.5rem;
    margin-bottom: 1rem;
    padding: 1rem;
}

.vs-field-header {
    align-items: center;
    display: flex;
    justify-content: space-between;
    margin-bottom: 0.5rem;
}

.vs-field-name {
    color: #6c757d;
    font-size: 0.875rem;
}

.vs-field-validation {
    font-size: 0.875rem;
    margin-top: 0.25rem;
}
```

## What You Can Customize

Use `field` to control the editable Razor markup:

- `field.Name`: the model property name.
- `field.Id`: the generated input id used by labels.
- `field.Label`: the `DisplayAttribute` label after localization.
- `field.ShowLabel`: whether the active layout options want a label.
- `field.Input`: the generated input component with binding intact.
- `field.ValidationMessage`: the generated validation message.
- `field.FieldDefinition`: the underlying VxForm field definition.

This gives Visual Studio users a normal `.razor` file to edit, while avoiding generated code that can drift from the model and library internals.
