# VxFormGenerator

VxFormGenerator is a Blazor form-generation library. It renders form fields inside a normal Blazor `EditForm` instead of wrapping the `EditForm`, so applications keep control over submit handling, validation, buttons, layout, and surrounding markup.

The current codebase targets `net10.0` and supports two form-generation paths:

- Model-based forms from annotated POCO models.
- Dynamic metadata forms from `VxFormModelDefinition`, designed to work in Blazor Server, Blazor WebAssembly, AOT, and restricted runtimes.

`ExpandoObject` rendering is no longer the dynamic form path. Use metadata rendering for portable dynamic forms.

## Choose A Form Path

- Use model-based rendering when the form comes from a compiled POCO with data annotations and optional VxFormGenerator layout attributes.
- Use schema-first dynamic rendering when the form shape is defined at runtime, loaded from configuration, shared across server and WebAssembly, or needs to stay compatible with AOT and restricted runtimes.
- Migration note: if older code still relies on `ExpandoObject` as the dynamic path, move that workflow to `VxFormModelDefinition` plus `VxFormMetadataBuilder` and `RenderVxFormMetadata`.

Start with the metadata demo route when evaluating the schema-first path:

- `/dynamic-form` shows the portable runtime-metadata workflow.
- `/definition-form` shows the compiled model/layout workflow.

## Packages

Choose one component package for your app:

```bash
dotnet add package VxFormGenerator.Components.Plain
```

```bash
dotnet add package VxFormGenerator.Components.Bootstrap
```

The Bootstrap package assumes Bootstrap is already loaded by the host app.

## Setup

Register VxFormGenerator in the app service collection.

Plain components:

```csharp
using VxFormGenerator.Settings.Plain;

builder.Services.AddVxFormGenerator();
```

Bootstrap components:

```csharp
using VxFormGenerator.Settings.Bootstrap;

builder.Services.AddVxFormGenerator();
```

Blazor WebAssembly example:

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddVxFormGenerator();

await builder.Build().RunAsync();
```

The repository includes a runnable Blazor WebAssembly demo in
`VxFormGeneratorDemo.Wasm`. It uses the shared demo pages and the same
component registration pattern shown above, so the model-based and
metadata-rendered examples can be checked in a browser without a server-side
Blazor host. For runtime-defined forms, use the `/dynamic-form` route; it
renders from `VxFormModelDefinition` metadata and avoids `Reflection.Emit`, so
it remains portable to WebAssembly, AOT, and restricted runtimes.

Blazor Server example:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddVxFormGenerator();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
```

## Model-Based Forms

Annotate a model with standard data annotations and optional VxFormGenerator layout attributes.

```csharp
public class FeedingSession
{
    [Display(Name = "Kind of food")]
    public FoodKind KindOfFood { get; set; }

    [Display(Name = "Note")]
    [MinLength(5)]
    public string Note { get; set; }

    [Display(Name = "Amount")]
    public decimal Amount { get; set; }

    [Display(Name = "Start")]
    public DateTime Start { get; set; }

    [Display(Name = "Throwing up")]
    public bool ThrowingUp { get; set; }

    [Display(Name = "Color")]
    public VxColor Color { get; set; }
}
```

Render the generated inputs inside an `EditForm`:

```razor
@using VxFormGenerator.Core
@using VxFormGeneratorDemoData

<EditForm Model="Model" OnValidSubmit="HandleValidSubmit">
    <ObjectGraphDataAnnotationsValidator />
    <RenderFormElements />
    <button class="btn btn-primary" type="submit">Submit</button>
</EditForm>

@code {
    private FeedingSession Model { get; } = new FeedingSession();

    private void HandleValidSubmit(EditContext context)
    {
        // Save the form data.
    }
}
```

Use `ObjectGraphDataAnnotationsValidator` when the model contains nested objects or collections that need recursive validation.

## Custom Field Templates

When a form needs custom HTML for CSS or business layout requirements, provide a `FieldTemplate`. The generator still creates the input component, binding, and validation message, while the app controls the surrounding markup.

```razor
<EditForm Model="Model" OnValidSubmit="HandleValidSubmit">
    <ObjectGraphDataAnnotationsValidator />
    <RenderFormElements>
        <FieldTemplate Context="field">
            <div class="custom-field" data-field-name="@field.Name">
                @if (field.ShowLabel)
                {
                    <label class="custom-label" for="@field.Id">@field.Label</label>
                }

                <div class="custom-input">
                    @field.Input
                </div>

                <div class="custom-validation">
                    @field.ValidationMessage
                </div>
            </div>
        </FieldTemplate>
    </RenderFormElements>
    <button class="btn btn-primary" type="submit">Submit</button>
</EditForm>
```

`FieldTemplate` is intentionally field-scoped for the first customization layer. Use `FormLayoutOptions` and layout attributes for row/group structure.

Visual Studio users who want an editable Razor component workflow can follow the copy/paste guide in [`Docs/visual-studio-field-template.md`](Docs/visual-studio-field-template.md).

The same guide also shows the `vxform scaffold` CLI workflow for generating a physical `.razor` file from a compiled model assembly.

## Layout

Use layout attributes when field order, rows, columns, labels, or placeholders need to be controlled from the model.

```csharp
[VxFormRowLayout(Id = 2, Label = "Address")]
public class AddressViewModel
{
    [Display(Name = "Firstname")]
    [VxFormElementLayout(RowId = 1)]
    public string SurName { get; set; }

    [Display(Name = "Lastname")]
    [VxFormElementLayout(RowId = 1, Placeholder = "Your lastname")]
    public string LastName { get; set; }

    [Display(Name = "Street")]
    [VxFormElementLayout(RowId = 2, ColSpan = 9)]
    [MinLength(5)]
    public string Street { get; set; }

    [Display(Name = "Number")]
    [VxFormElementLayout(RowId = 2, ColSpan = 3)]
    public string Number { get; set; }
}
```

Global layout options:

```csharp
builder.Services.AddVxFormGenerator(new VxFormLayoutOptions
{
    LabelOrientation = LabelOrientation.TOP,
    ShowPlaceholder = PlaceholderPolicy.EXPLICIT_LABEL_FALLBACK,
    VisualValidationPolicy = VisualFeedbackValidationPolicy.ONLY_INVALID
});
```

Component-level options override the global defaults:

```razor
<RenderFormElements FormLayoutOptions="OptionsForForm" />

@code {
    private VxFormLayoutOptions OptionsForForm { get; } = new VxFormLayoutOptions
    {
        LabelOrientation = LabelOrientation.LEFT
    };
}
```

## Nested Models

Mark nested model properties with `VxFormGroup` and validation attributes.

```csharp
public class OrderViewModel
{
    [VxFormGroup(Label = "Delivery")]
    [ValidateComplexType]
    public AddressViewModel Address { get; set; } = new AddressViewModel();

    [VxFormGroup(Label = "Invoice")]
    [ValidateComplexType]
    public AddressViewModel BillingAddress { get; set; } = new AddressViewModel();

    [Display(Name = "Send insured")]
    public bool SendInsured { get; set; }
}
```

## Schema-First Dynamic Forms

If your application defines fields at runtime, start with `VxFormModelDefinition`, build a metadata model, and render it with `RenderVxFormMetadata`.

Quick start:

```csharp
using VxFormGenerator.Core.Dynamic;

var definition = new VxFormModelDefinition
{
    Namespace = "MyApp.GeneratedForms",
    ClassName = "CustomerForm"
};

definition.Properties.Add(new VxFormModelPropertyDefinition
{
    Name = "FirstName",
    TypeName = "string",
    Label = "First name",
    IsRequired = true
});

var metadataModel = VxFormMetadataBuilder.Build(definition);
```

```razor
<RenderVxFormMetadata Model="metadataModel" />
```

For dynamic forms, build a `VxFormModelDefinition` at runtime and render it with `RenderVxFormMetadata`.

```csharp
using VxFormGenerator.Core.Dynamic;

var definition = new VxFormModelDefinition
{
    Namespace = "MyApp.GeneratedForms",
    ClassName = "CustomerForm"
};

definition.Properties.Add(new VxFormModelPropertyDefinition
{
    Name = "FirstName",
    Id = "customer-first-name",
    TypeName = "string",
    Label = "First name",
    Placeholder = "Your first name",
    RowId = 1,
    RowLabel = "Customer",
    ColSpan = 6,
    Order = 10,
    IsRequired = true,
    MinLength = 2,
    MaxLength = 40,
    DefaultValueExpression = "string.Empty"
});

definition.Properties.Add(new VxFormModelPropertyDefinition
{
    Name = "Servings",
    TypeName = "int?",
    Label = "Servings",
    RowId = 1,
    ColSpan = 3
});
```

Add lookup/dropdown options:

```csharp
var foodKind = new VxFormModelPropertyDefinition
{
    Name = "FoodKind",
    Id = "food-kind",
    TypeName = "string",
    Label = "Food kind",
    FieldKind = VxFormFieldKind.Select,
    RowId = 2,
    ColSpan = 6
};

foodKind.Options.Add(new VxFormLookupOption { Value = "Bottle", Label = "Bottle", IsSelected = true });
foodKind.Options.Add(new VxFormLookupOption { Value = "Solid", Label = "Solid food" });
foodKind.Options.Add(new VxFormLookupOption { Value = "Other", Label = "Other" });

definition.Properties.Add(foodKind);
```

Add conditional visibility:

```csharp
definition.Properties.Add(new VxFormModelPropertyDefinition
{
    Name = "OtherFood",
    TypeName = "string",
    Label = "Other food",
    RowId = 2,
    ColSpan = 6,
    VisibilityRule = new VxFormVisibilityRule
    {
        SourceField = "FoodKind",
        EqualsValue = "Other"
    }
});
```

Build and render the metadata model:

```csharp
var metadataModel = VxFormMetadataBuilder.Build(definition);
```

```razor
<RenderVxFormMetadata Model="metadataModel" />
```

Submitted values are stored in `VxFormMetadataModel.Values`.

Supported metadata features include:

- Stable field ids for accessible label/input associations.
- Row and column layout through `RowId`, `RowLabel`, `ColSpan`, and `Order`.
- Text, number, date, checkbox, and select rendering.
- Lookup options with labels, selected defaults, and disabled choices.
- Nullable primitive aliases such as `int?`, `decimal?`, `datetime?`, and `bool?`.
- Simple equality-based conditional visibility.

The demo route `/dynamic-form` intentionally uses metadata rendering so the same dynamic form path works in both the Blazor Server and Blazor WebAssembly demos.

## Runtime Types And Source Text

Metadata rendering does not require runtime CLR type generation.

When a server-side scenario specifically needs a reflected CLR type with attributes, use:

```csharp
var modelType = VxFormRuntimeModelBuilder.BuildType(definition);
var instance = VxFormRuntimeModelBuilder.CreateInstance(definition);
```

`VxFormRuntimeModelBuilder` uses `Reflection.Emit`, so do not use it as the portable Blazor WebAssembly path.

When source text is useful for diagnostics, persistence, or build-time generation, use:

```csharp
var source = VxFormModelSourceGenerator.Generate(definition);
```

## Localization

Model-based forms honor localized `DisplayAttribute` and validation resources.

```csharp
public class AddressViewModel
{
    [Display(ResourceType = typeof(Resources.Address), Name = nameof(Resources.Address.FIRSTNAME_LABEL))]
    [VxFormElementLayout(RowId = 1)]
    public string SurName { get; set; }

    [Display(Name = "Street")]
    [MinLength(5,
        ErrorMessageResourceType = typeof(Resources.Address),
        ErrorMessageResourceName = nameof(Resources.Address.STREET_MIN_LENGTH))]
    public string Street { get; set; }
}
```

Server-side localization setup example:

```csharp
builder.Services.AddLocalization();

var supportedCultures = new[] { "en", "nl" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
```

The demo data project includes English and Dutch address resources.

## Development Container

The repository includes a devcontainer under `.devcontainer/`. It uses the .NET 10 SDK image with ICU installed, so localization tests can run without `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1`.

Open the repository in VS Code and choose `Dev Containers: Reopen in Container`, or build the image manually:

```bash
docker build -f .devcontainer/Dockerfile -t vxformgenerator-devcontainer .
```

## Run Demos

Server demo:

```bash
dotnet run --project VxFormGeneratorDemo.Server/VxFormGeneratorDemo.Server.csproj
```

WebAssembly demo:

```bash
dotnet run --project VxFormGeneratorDemo.Wasm/VxFormGeneratorDemo.Wasm.csproj
```

The WebAssembly demo serves the same useful routes listed below. Start with
`/dynamic-form` when validating schema-first dynamic forms in the browser.

Useful routes:

- `/` for the model-based demo.
- `/definition-form` for layout options.
- `/dynamic-form` for metadata-rendered dynamic forms.

## Tests

Run the core test suite:

```bash
dotnet test VxFormGenerator.Core.Tests/VxFormGenerator.Core.Tests.csproj
```

The test suite covers metadata rendering, nullable values, lookup options, conditional visibility, source/runtime model generation, object-graph validation, and localization resources.

## Contact

<img src="https://github.com/Aaltuj/VxFormGenerator/blob/master/Docs/images/discord-logo.png" alt="Discord" /> [Server](https://discord.gg/pyCtvFdTdV)

## Community

- [Contributing guide](CONTRIBUTING.md)
- [Code of conduct](CODE_OF_CONDUCT.md)
- [Security policy](SECURITY.md)
