# Visual Studio Field Template Workflow

Issue #29 asked for a Visual Studio-friendly way to start from an annotated POCO and get editable Razor markup.

VxFormGenerator supports two practical workflows:

- Use the CLI scaffolder to generate a physical `.razor` file that Visual Studio users can edit.
- Use `FieldTemplate` manually in a normal Razor component when you want to write the component yourself.

The generated component still uses VxFormGenerator for field discovery, input creation, binding, and validation messages.

## CLI Scaffold Workflow

1. Build the project that contains the annotated model.
2. Run the `vxform scaffold` command against the compiled assembly.
3. Add the generated `.razor` file to your app and customize the markup in Visual Studio.

```bash
dotnet run --project VxFormGenerator.Tools/VxFormGenerator.Tools.csproj -- scaffold \
  --assembly ./VxFormGeneratorDemoData/bin/Debug/net10.0/VxFormGeneratorDemoData.dll \
  --type VxFormGeneratorDemoData.AddressViewModel \
  --output ./VxFormGeneratorDemo.Shared/Pages/AddressGeneratedForm.razor
```

To generate the smaller default form without a field template:

```bash
dotnet run --project VxFormGenerator.Tools/VxFormGenerator.Tools.csproj -- scaffold \
  --assembly ./VxFormGeneratorDemoData/bin/Debug/net10.0/VxFormGeneratorDemoData.dll \
  --type VxFormGeneratorDemoData.AddressViewModel \
  --output ./VxFormGeneratorDemo.Shared/Pages/AddressGeneratedForm.razor \
  --no-field-template
```

The generated file is intentionally normal Razor code. It is not overwritten unless you run the command again for the same output path.

## Manual Visual Studio Workflow

If you do not want to use the CLI, create the component yourself:

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

## Manual Razor Component

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
