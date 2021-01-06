# VxFormGenerator

The library contains a component, that nests itself into the Blazor EditForm instead of a wrapper around the EditForm. The component is able to generate a form based on a POCO or a ExpandoObject. Because of this architecture the library provides the developer flexibility and direct usage of the EditForm. 

# TLDR

Turns an annotated object....

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
    
    [Display(Name = "End")]
    public DateTime End { get; set; }
    
    [Display(Name = "Throwing up")]
    public bool ThrowingUp { get; set; }

    [Display(Name = "Throwing up dict")]
    public ValueReferences<FoodKind> ThrowingUpDict { get; set; } = new ValueReferences<FoodKind>();
    
    [Display(Name = "Color")]
    public VxColor Color { get; set; }
}
```

... into a nice Blazor form:

![A nice form!](https://github.com/Aaltuj/VxFormGenerator/blob/master/Docs/sample_form.png)

### Setup

Add the NuGet package.

Open a terminal in the project folder where you want to add the **VxFormGenerator**. Pick one of the options below:

###### Plain components

The unstyled version of the input components for the **VxFormGenerator**

`dotnet add package VxFormGenerator.Components.Plain`

###### Bootstrap components

> The assumption made by the library is that you already added  Bootstrap (4.5.0 and up) setup in your Blazor app

The Bootstrap styled form components for the **VxFormGenerator**

`dotnet add package VxFormGenerator.Components.Bootstrap`

### Initialize

Open the `Startup.cs` and add one of the following usage statements:

###### Plain components

`using VxFormGenerator.Settings.Plain;`

###### Bootstrap components

`using VxFormGenerator.Settings.Bootstrap;`

After adding one of the usage statements add the line `services.AddVxFormGenerator();` like shown here below.

````C#
public IConfiguration Configuration { get; }

// This method gets called by the runtime. 
// Use this method to add services to the container.
// For more information on how to configure your application, 
// visit https://go.microsoft.com/fwlink/?LinkID=398940
public void ConfigureServices(IServiceCollection services)
{
	services.AddRazorPages();
	services.AddServerSideBlazor();
	services.AddVxFormGenerator();
}
````



### Model based

You can have a model that renders inputs for the properties. All that's required is adding a `RenderFormElements` component to the `EditForm`. The inputs can be validated by the attached Data Annotations on the property. Just add the built-in `DataAnnotationsValidator` component.

````html
@page "/"

@using VxFormGenerator.Core
@using FormGeneratorDemo.Data
@using System.Dynamic

<EditForm Model="Model" 
		  OnValidSubmit="HandleValidSubmit"
		  OnInvalidSubmit="HandleInValidSubmit">
    <DataAnnotationsValidator></DataAnnotationsValidator>
    <RenderFormElements></RenderFormElements>		
    <button class="btn btn-primary" type="submit">Submit</button>
</EditForm>

@code{

    /// <summary>
    /// Model that is used for the form
    /// </summary>
    private FeedingSession Model = new FeedingSession();

    /// <summary>
    /// Will handle the submit action of the form
    /// </summary>
    /// <param name="context">The model with values as entered in the form</param>
    private void HandleValidSubmit(EditContext context)
    {
        // save your changes
    }

    private void HandleInValidSubmit(VEditContext context)
    {
        // Do something
    }

}

````



### Dynamic based

You can render a form that is based on a dynamic `ExpandoObject`. The developer is that able to create a model based at runtime. All that's required is adding a `RenderFormElements` component to the `EditForm`. The inputs can **NOT** **YET** be validated by Data Annotations. This is a feature yet to be completed.

````html
@page "/"

@using VxFormGenerator.Core
@using FormGeneratorDemo.Data
@using System.Dynamic

<EditForm Model="Model" 
		  OnValidSubmit="HandleValidSubmit"
		  OnInvalidSubmit="HandleInValidSubmit">
    <DataAnnotationsValidator></DataAnnotationsValidator>
    <RenderFormElements></RenderFormElements>		
    <button class="btn btn-primary" type="submit">Submit</button>
</EditForm>

@code{

    /// <summary>
    /// Model that is used for the form
    /// </summary>
    private dynamic Model = new ExpandoObject();

	/// <summary>
	/// Create a dynamic object 
	/// </summary>
    protected override void OnInitialized()
    {
        var dict = (IDictionary<String, Object>) Model;
        dict.Add("Name", "add");
        dict.Add("Note", "This is a note");
        dict.Add("Date", DateTime.Now);
        dict.Add("Amount", 1);
    }

    /// <summary>
    /// Will handle the submit action of the form
    /// </summary>
    /// <param name="context">The model with values as entered in the form</param>
    private void HandleValidSubmit(EditContext context)
    {
        // save your changes
    }

    private void HandleInValidSubmit(VEditContext context)
    {
        // Do something
    }

}

````



### Layout 

The form generator supports layout structuring based on meta-data defined at model level.

````C#
        // Add label to row 2
    [VxFormRowLayout(Id = 2, Label = "Adress")]
    public class AddressViewModel
    {
        [Display(Name = "Firstname")]
        // Add element to row 1 with automatic width based on number of items in a row
        [VxFormElementLayout(RowId = 1)]
        public string SurName { get; set; }
        // Add element to row 1 with automatic width based on number of items in a row and define a placeholder
        [VxFormElementLayout(RowId = 1, Placeholder = "Your Lastname")]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "Street")]
        // Add element to row 2 and set the width to 9 of 12 columns
        [VxFormElementLayout(RowId = 2, ColSpan = 9)]
        [MinLength(5)]
        public string Street { get; set; }

        [Display(Name = "Number")]
        // Add element to row 2 and set the width to 3 of 12 columns
        [VxFormElementLayout(RowId = 2, ColSpan = 3)]
        public string Number { get; set; }

        [Display(Name = "Country"),
         // Show Placeholder
         VxFormElementLayout(Placeholder = "The country you live")]
        public string Country { get; set; }

        [Display(Name = "State")]
        [MinLength(5)]
        public string State { get; set; }

    }
````
![Another nice form!](https://github.com/Aaltuj/VxFormGenerator/blob/master/Docs/Advanced_sample_form.png)

There is also support for nested models. 

```C#
  public class OrderViewModel
    {
         // Indicate that this property type should be rendered as a separate elements in the form and give it a label
        [VxFormGroup(Label = "Delivery")]
        // Use this to valdidate a complex object
        [ValidateComplexType]
        public AddressViewModel Address { get; set; } = new AddressViewModel();

        // Indicate that this property type should be rendered as a separate elements in the form and give it a label
        [VxFormGroup(Label = "Invoice")]
        // Use this to valdidate a complex object
        [ValidateComplexType]
        public AddressViewModel BillingAddress { get; set; } = new AddressViewModel();

        [Display(Name = "Send insured")]
        public bool Valid { get; set; } = true;

        [Display(Name = "What color box")]
        public VxColor Color { get; set; }
    }
    }
```
![Another Another nice form!](https://github.com/Aaltuj/VxFormGenerator/blob/master/Docs/complex_sample_form.png)


#### Layout options

The form support multiple rendering options:

Set options **Global**

```
  public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddVxFormGenerator(new VxFormLayoutOptions() { LabelOrientation = LabelOrientation.TOP });
        }
```

Set options at **Component** level, these options override the global one.

```html
 <RenderFormElements FormLayoutOptions="@OptionsForForm"></RenderFormElements>

@code{
    private VxFormLayoutOptions OptionsForForm = new VxFormLayoutOptions();
}
```

#### Possible options

*Set the label position for the form*

**Label position**:          Top | Left | None

*Set the placeholder policy for the form*

**Placeholder Policy**: Explicit | Implicit | None | ExplicitFallbackToLabels | ImplicitFallbackToLabels

*Set the trigger for showing validation state*

**Validation Policy**:     OnlyValid | OnlyInvalid | BothValidAndInvalid



### Run demo

Run the demo so you can see the options and effects interactively:

1. `git clone https://github.com/Aaltuj/VxFormGenerator.git`
2. `cd VxFormGenerator ` 
3. `run.cmd` on Windows or `bash run.sh` on Linux/Mac
4. navigate to `http://localhost:5000/definition-form`



### Apply your own styling

> This is a work in progress



### Contact

<img src="https://github.com/Aaltuj/VxFormGenerator/blob/master/Docs/discord-logo.png" alt="Discord" /> [Server](https://discord.gg/pyCtvFdTdV)

