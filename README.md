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



### Apply your own styling

> This is a work in progress