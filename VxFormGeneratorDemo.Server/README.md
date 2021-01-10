# Build a form-generator with Blazor

My journey into Blazor started with the birth of my lovely daughter on 20-02-2020. The little dino-baby (one of her nicknames 😉) was eating, sleeping and being cute. As a new parent a lot of stuff is coming your way. You need to keep up with many things, especially the last time the baby had their milk. Looking at the apps that help you track these, any many other events, I had found  that a lot require me to create an account and were sharing my data. Because don't want to store that data somewhere other people could use it, I thought: let's built my own. 

### Babysteps

Tracking your baby requires data, that data needs to be collected, and how do you collect that data? 

![Form gif](https://media.giphy.com/media/8p8E1sylIARDW/source.gif)

So we need forms. Let's create a feeding session form in Blazor.

We'll start with a model that represents a feeding session based on time.

````C#
  public class FeedingSession
    {      
        public FoodKind KindOfFood { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public enum FoodKind
    {
        SOLID,
        LIQUID
    }
````

Now let's create a razor page that allows to fill these properties with values.

````html
@page "/"

<div class="container">
    <h2>Show me my milky</h2>
 <EditForm  
 Model="@session">
            <DataAnnotationsValidator />
            <ValidationSummary />
    <div class="form-group">
        <label for="note">Note</label>
        <InputText id="note" class="form-control" @bind-Value="session.Note" />
         <ValidationMessage For="@(() => session.Note)" />
    </div>
    <div class="form-group">
        <label for="note">Amount</label>
        <InputNumber id="amount" class="form-control" @bind-Value="session.Amount" />
        <ValidationMessage For="@(() => session.Amount)" />

    </div>
    <div class="form-group">
        <label for="start">Start</label>
        <input id="start" type="datetime"
        class="form-control" 
        @bind="session.Start"/>
         <ValidationMessage 
        For="@(() => session.Start)" />
    </div>
    <div class="form-group">
        <label for="end">End</label>
        <input id="end" type="datetime" class="form-control" @bind="session.End"/>
         <ValidationMessage For="@(() => session.End)" />
    </div>
    <div class="form-group">
        <label for="kind">Kind of Food</label>
        <select id="kind" class="form-control" @bind="session.KindOfFood" >
            <option value="SOLID">Solid</option>
            <option value="LIQUID">Liquid</option>
        </select>
        <ValidationMessage For="@(() => session.KindOfFood)" />
    </div>
 </EditForm>

 <!-- Output -->
 <div class="card">
     <div class="card-header">
         Output
     </div>
  <div class="card-body">
    <div>
        <label class="mr-2">Note:</label>
        <label>@session.Note</label>
    </div>
     <div>
        <label class="mr-2">Amount:</label>
        <label>@session.Amount</label>
    </div>
     <div>
        <label class="mr-2">Start:</label>
        <label>@session.Start</label>
    </div>
     <div>
        <label class="mr-2">End:</label>
        <label>@session.End</label>
    </div>
     <div>
        <label class="mr-2">Kind of food:</label>
        <label>@session.KindOfFood</label>
    </div>
  </div>
</div>
</div>

@code{

 private FeedingSession session = new FeedingSession();

}
````

> Working example found on BlazorFiddle [here](https://blazorfiddle.com/s/ohayilks)



### A little growth jump

Having this form functional I wasn't happy with all the necessary manual bootstrapping. The app will have a least 10 models that needed to be a form. The idea of creating and maintaining them wasn't something I was looking forward to.

Fortunately In Angular I've created multiple form generators so it was time to explore the dynamic way. The idea is to generate forms based on POCO's and data-annotations. Having the experience with the built-in [EditForm](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.editform?view=aspnetcore-3.1) I wanted to leverage as much of the out-of-box behaviour, because there is no need to reinvent the wheel. 

#### Requirements

- **Must** work with [EditForm](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.editform?view=aspnetcore-3.1) 

  > So we can use the built-in validation and form handling logic

- **Must** be based on [InputBase](https://github.com/dotnet/aspnetcore/blob/c79002bf38f2a08f496c645ee04e0a9084601f31/src/Components/Web/src/Forms/InputBase.cs)

  > So the [EditForm](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.editform?view=aspnetcore-3.1) is able to use it's built-in validation and change handling logic

- CustomFormElements **Must** be bind-able

  > The form fields that are custom implemented like a datepicker should be able to use the `bind-Value` syntax

- CustomFormElements **Must** have input validation

  > The input validation should be extendable from the [InputBase](https://github.com/dotnet/aspnetcore/blob/c79002bf38f2a08f496c645ee04e0a9084601f31/src/Components/Web/src/Forms/InputBase.cs)

- **Must** be generated by a POCO

  > The form must use reflection to list all properties and it's decorators to render them as form fields

- **Should** be extendable with custom components

  > The form should have a repository that contains a mapping to a component that should be used as form element the form when a property is of a specific [Type](https://docs.microsoft.com/en-us/dotnet/api/system.type?view=netcore-3.1)

- **Should** be easy to change CSS framework

  > The form should be able to switch, for example, to: Bootstrap or Material Design



### Implementation

The intend it that I lay down the basic groundwork for a fully featured form generator.  So don't expect a fully grown baby yet 😉.

Now that is out of the way let's start by creating a new folder and component. 

1. Navigate to project root of you Blazor application

2. Create the folders `Components\FormGenerator`

3. Create a new  `FormGenerator.razor` file

4. ````HTML
   @typeparam TValue
   @inherits FormGeneratorComponent<TValue>
   
   
   <EditForm Model="@DataContext"
             OnValidSubmit="@(context => OnValidSubmit.InvokeAsync(context))">
       <DataAnnotationsValidator />
       <ValidationSummary />
   
       @foreach (var field in Properties)
       {
           <FormElement FieldIdentifier="@field"></FormElement>
       }
   
       <button type="submit">Submit</button>
   </EditForm>
   ````
   
5. Create a new  `FormGenerator.razor.cs` file

   *(Make sure it's in the same place as the `.razor` file)*

6. ````c#
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
   ````
   
7. We will need to have a loader component, so let create a new file `FormElement.razor`

8. ````html
   @inherits FormElementComponent
   
   @if (!string.IsNullOrWhiteSpace(Label))
   {
       <label class="form-control-label" for="@Id">@Label</label>
   }
   
   @CreateComponent(FieldIdentifier)
   
   ````

9. Let's create the backend file `FormGenerator.razor.cs` 

   *(Make sure it's in the same place as the `.razor` file)*

10. ````C#
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.AspNetCore.Components.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    
    namespace FormGeneratorDemo.Components.FormGenerator
    {
        public class FormElementComponent : OwningComponentBase
        {
            private string _Label;
            private FormGeneratorComponentsRepository _repo;
            /// <summary>
            /// Bindable property to set the class
            /// </summary>
            public string CssClass { get => string.Join(" ", CssClasses.ToArray()); }
            /// <summary>
            /// Setter for the classes of the form container
            /// </summary>
            [Parameter] public List<string> CssClasses { get; set; }
    
            /// <summary>
            /// Will set the 'class' of the all the controls. Useful when a framework needs to implement a class for all form elements
            /// </summary>
            [Parameter] public List<string> DefaultFieldClasses { get; set; }
            /// <summary>
            /// The identifier for the <see cref="FormElement"/>"/> used by the label element
            /// </summary>
            [Parameter] public string Id { get; set; }
    
            /// <summary>
            /// The label for the <see cref="FormElement"/>, if not set, it will check for a <see cref="DisplayAttribute"/> on the <see cref="CascadedEditContext.Model"/>
            /// </summary>
            [Parameter]
            public string Label
            {
                get
                {
                    var dd = CascadedEditContext.Model
                         .GetType()
                         .GetProperty(FieldIdentifier.Name)
                         .GetCustomAttributes(typeof(DisplayAttribute), false)
                         .FirstOrDefault() as DisplayAttribute;
    
                    return _Label ?? dd?.Name;
                }
                set { _Label = value; }
            }
    
            /// <summary>
            /// The property that should generate a formcontrol
            /// </summary>
            [Parameter] public PropertyInfo FieldIdentifier { get; set; }
    
            /// <summary>
            /// Get the <see cref="EditForm.EditContext"/> instance. This instance will be used to fill out the values inputted by the user
            /// </summary>
            [CascadingParameter] EditContext CascadedEditContext { get; set; }
    
            protected override void OnInitialized()
            {
                // setup the repo containing the mappings
                _repo = ScopedServices.GetService(typeof(FormGeneratorComponentsRepository)) as FormGeneratorComponentsRepository;
            }
    
            /// <summary>
            /// A method thar renders the form control based on the <see cref="FormElement.FieldIdentifier"/>
            /// </summary>
            /// <param name="propInfoValue"></param>
            /// <returns></returns>
            public RenderFragment CreateComponent(System.Reflection.PropertyInfo propInfoValue) => builder =>
            {
                // Get the mapped control based on the property type
                var componentType = _repo.GetComponent(propInfoValue.PropertyType.ToString());
    
    
                if (componentType == null)
                    throw new Exception($"No component found for: {propInfoValue.PropertyType.ToString()}");
    
                // Set the found component
                var elementType = componentType;
    
                // When the elementType that is rendered is a generic Set the propertyType as the generic type
                if (elementType.IsGenericTypeDefinition)
                {
                    Type[] typeArgs = { propInfoValue.PropertyType };
                    elementType = elementType.MakeGenericType(typeArgs);
                }
    
                // Activate the the Type so that the methods can be called
                var instance = Activator.CreateInstance(elementType);
    
            // Get the generic CreateFormComponent and set the property type of the model and the elementType that is rendered
                MethodInfo method = typeof(FormElementComponent).GetMethod(nameof(FormElementComponent.CreateFormComponent));
                MethodInfo genericMethod = method.MakeGenericMethod(propInfoValue.PropertyType, elementType);
                // Execute the method with the following parameters
                genericMethod.Invoke(this, new object[] { this, CascadedEditContext.Model, propInfoValue, builder, instance });
            };
    
            /// <summary>
            /// Creates the component that is rendered in the form
            /// </summary>
            /// <typeparam name="T">The type of the property</typeparam>
            /// <typeparam name="TElement">The type of the form element, should be based on <see cref="InputBase{TValue}"/>, like a <see cref="InputText"/></typeparam>
            /// <param name="target">This <see cref="FormElement"/> </param>
            /// <param name="dataContext">The Model instance</param>
            /// <param name="propInfoValue">The property that is being rendered</param>
            /// <param name="builder">The render tree of this element</param>
            /// <param name="instance">THe control instance</param>
            public void CreateFormComponent<T, TElement>(object target,
                object dataContext,
                PropertyInfo propInfoValue, RenderTreeBuilder builder, InputBase<T> instance)
            {
                // Create the component based on the mapped Element Type
                builder.OpenComponent(0, typeof(TElement));
    
                // Bind the value of the input base the the propery of the model instance
                var s = propInfoValue.GetValue(dataContext);
                builder.AddAttribute(1, nameof(InputBase<T>.Value), s);
    
                // Create the handler for ValueChanged. This wil update the model instance with the input
                builder.AddAttribute(3, nameof(InputBase<T>.ValueChanged),
                        Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(
                            EventCallback.Factory.Create<T>(
                                target, EventCallback.Factory.
                                CreateInferred(target, __value => propInfoValue.SetValue(dataContext, __value),
                                (T)propInfoValue.GetValue(dataContext)))));
    
                // Create an expression to set the ValueExpression-attribute.
                var constant = Expression.Constant(dataContext, dataContext.GetType());
                var exp = Expression.Property(constant, propInfoValue.Name);
                var lamb = Expression.Lambda<Func<T>>(exp);
                builder.AddAttribute(4, nameof(InputBase<T>.ValueExpression), lamb);
    
                // Set the class for the the formelement.
                builder.AddAttribute(5, "class", GetDefaultFieldClasses(instance));
    
                CheckForInterfaceActions<T, TElement>(this, CascadedEditContext.Model, propInfoValue, builder, instance, 6);
    
                builder.CloseComponent();
    
            }
    
            private void CheckForInterfaceActions<T, TElement>(object target,
                object dataContext,
                PropertyInfo propInfoValue, RenderTreeBuilder builder, InputBase<T> instance, int indexBuilder)
            {
                // overriding the default classes for FormElement
                if (TypeImplementsInterface(typeof(TElement), typeof(IRenderAsFormElement)))
                {
                    this.CssClasses.AddRange((instance as IRenderAsFormElement).FormElementClasses);
                }
    
                // Check if the component has the IRenderChildren and renderen them in the form control
                if (TypeImplementsInterface(typeof(TElement), typeof(IRenderChildren)))
                {
                    (instance as IRenderChildren).RenderChildren(builder, indexBuilder, dataContext, propInfoValue);
                }
            }
    
            /// <summary>
            /// Merges the default control classes with the <see cref="InputBase{TValue}.AdditionalAttributes"/> 'class' key
            /// </summary>
            /// <typeparam name="T">The property type of the formelement</typeparam>
            /// <param name="instance">The instance of the component representing the form control</param>
            /// <returns></returns>
            private string GetDefaultFieldClasses<T>(InputBase<T> instance)
            {
    
                var output = DefaultFieldClasses == null ? "" : string.Join(" ", DefaultFieldClasses);
    
                if (instance == null)
                    return output;
    
                var AdditionalAttributes = instance.AdditionalAttributes;
    
                if (AdditionalAttributes != null &&
                      AdditionalAttributes.TryGetValue("class", out var @class) &&
                      !string.IsNullOrEmpty(Convert.ToString(@class)))
                {
                    return $"{@class} {output}";
                }
    
                return output;
            }
    
            private bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
            {
                if (typeToCheck == typeof(object))
                {
                    return false;
                }
                else if (typeToCheck == null)
                {
                    return false;
                }
                else if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
                else
                {
                    return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
                }
            }
            private static bool TypeImplementsInterface(Type type, Type typeToImplement)
            {
                Type foundInterface = type
                    .GetInterfaces()
                    .Where(i =>
                    {
                        return i.Name == typeToImplement.Name;
                    })
                    .Select(i => i)
                    .FirstOrDefault();
    
                return foundInterface != null;
            }
    
        }
    }
    ````
    
11. Now let's create a component mapping, we use the Type of the property to show a corresponding HTML Element represented by a component.

12. Create in the `FormGenerator` folder a new file  `FormGeneratorComponentsRepository.cs`

13. ````C#
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    namespace FormGeneratorDemo.Components.FormGenerator
    {
    
        public class FormGeneratorComponentsRepository
        {
            private Dictionary<string, Type> _ComponentDict = new Dictionary<string, Type>();
    
            public Type _DefaultComponent { get; private set; }
    
            public FormGeneratorComponentsRepository()
            {
    
            }
            public FormGeneratorComponentsRepository(Dictionary<string, Type> componentRegistrations, Type defaultComponent)
            {
                _ComponentDict = componentRegistrations;
                _DefaultComponent = defaultComponent;
            }
    
            public void RegisterComponent(string key, Type component)
            {
                _ComponentDict.Add(key, component);
            }
    
            public void RemoveComponent(string key)
            {
                _ComponentDict.Remove(key);
            }
    
            public Type GetComponent(string key)
            {
                Type outVar = null;
    
                _ComponentDict.TryGetValue(key, out outVar);
    
                return outVar ?? _DefaultComponent;
            }
    
            public void Clear()
            {
                _ComponentDict.Clear();
            }
        }
    }
    
    ````

14. Let's setup the mapping for the form, open `Startup.cs` and register the component mapping repo as a singleton to the Dependency Injection system.

15. ````c#
    public void ConfigureServices(IServiceCollection services)
            {           
                 services.AddSingleton(new FormGeneratorComponentsRepository(
                      new Dictionary<string, Type>()
                      {
                            {typeof(string).ToString(), typeof(InputText) },
                            {typeof(DateTime).ToString(), typeof(InputDate<>) },
                            {typeof(bool).ToString(), typeof(InputCheckbox) },
                          	{typeof(FoodKind).ToString(), typeof(InputSelect<>) },
                            {typeof(decimal).ToString(), typeof(InputNumber<>) }
                      }, null));
            }
    
    
    ````

16. This will, for example, render properties of type: String as a Blazor InputText component

17. Let's use it

18. ```` html
    @page "/"
    
    @using FormGeneratorDemo.Components.FormGenerator
    @using FormGeneratorDemo.Data
    
    <h1>Hello, world!</h1>
    
    Welcome to your new app.
    
    <!-- Bind the session instance of the FeedingSession to the DataContext parameter
         Also hook up the event when the submit button is clicked -->
    <FormGenerator DataContext="session"
                   OnValidSubmit="HandleValidSubmit"></FormGenerator>
    
    <!-- Output -->
    <div class="card mt-2">
        <div class="card-header">
            Output
        </div>
        <div class="card-body">
            <div>
                <label class="mr-2">Note:</label>
                <label>@session.Note</label>
            </div>
            <div>
                <label class="mr-2">Amount:</label>
                <label>@session.Amount</label>
            </div>
            <div>
                <label class="mr-2">Start:</label>
                <label>@session.Start</label>
            </div>
            <div>
                <label class="mr-2">End:</label>
                <label>@session.End</label>
            </div>
            <div>
                <label class="mr-2">Kind of food:</label>
                <label>@session.KindOfFood</label>
            </div>
        </div>
    </div>
    
    @code{
    
        /// <summary>
        /// Model that is used for the form
        /// </summary>
        private FeedingSession session = new FeedingSession();
    
        /// <summary>
        /// Will handle the submit action of the form
        /// </summary>
        /// <param name="context">The model with values as entered in the form</param>
        private void HandleValidSubmit(EditContext context)
        {
            // save your changes
        }
    
    }
    ````

Running this application will result in a un-styled rendered form generated by a POCO!!! Unfortunately the  the `InputSelect` is not rendering the  `KindOfFood` enum as possible options for the `select` element. 

![image-20200705221712944](.\Build a form-generator in Blazor.assets\image-20200705221712944.png)

This is because `InputSelect`  expects `<option>`  elements as it's child. So we need to append children dynamically to the `InputSelect`  component.

````html
 <InputSelect @bind-Value="session.KindOfFood">
                <option value="SOLID">SOLID</option>
                <option value="LIQUID">LIQUID</option>
 </InputSelect>
````

### Append the children

There are multiple ways to create an architecture for this. I've chosen the following architecture that results in a simple and scoped solution:

- Leaving the `FormGeneratorComponentsRepository` unchanged
- Adding a interface called `IRenderChildren`
- Extend the `FormElement` to check for the `IRenderChildren` interface
- Extend the `FormElement` to execute the `renderChildren` methods with the correct parameters

Let's add the support for rendering children:

1. Create a new file in `FormGenerator` folder called: `IRenderChildren.cs`

````C#
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace FormGeneratorDemo.Components.FormGenerator
{
    /// <summary>
    /// Helper interface for rendering values in components, needs to be non-generic for the form generator
    /// </summary>
    public interface IRenderChildren
    {
        /// <summary>
        /// Let the form generator know what element to render    
        /// </summary>
        Type TypeToRender { get; }
        /// <summary>
        /// Function that will render the children for <see cref="TypeToRender"/>
        /// </summary>
        /// <typeparam name="TElement">The element type of the <see cref="TypeToRender"/></typeparam>
        /// <param name="builder">The builder for rendering a tree</param>
        /// <param name="index">The index of the element</param>
        /// <param name="dataContext">The model for the form</param>
        /// <param name="propInfoValue">The property that is filled by the <see cref="FormElement"/></param>
        void RenderChildren<TElement>(RenderTreeBuilder builder, int index, object dataContext,
            PropertyInfo propInfoValue);
    }
}
````

2. Create a new file in `FormGenerator` folder called: `InputSelectOption.razor`. This will be the `option` HTML element.

3. ````C#
   @typeparam TKey
   
   <option value="@Key">@Value</option>
   
   @code {
       [Parameter] public string Value { get; set; }
       [Parameter] public TKey Key { get; set; }
   }
   ````

4. Now let's use this component to render it in the `InputSelect` component

5. Create a new file in `FormGenerator` folder called: `RenderChildrenInputSelect.cs`

6. ````c#
   using Microsoft.AspNetCore.Components;
   using Microsoft.AspNetCore.Components.Forms;
   using Microsoft.AspNetCore.Components.Rendering;
   using System;
   using System.Reflection;
   
   namespace FormGeneratorDemo.Components.FormGenerator
   {
       public class RenderChildrenInputSelect : IRenderChildren
       {
           public Type TypeToRender =>typeof(InputSelect<>);
   
           public void RenderChildren<TValue>(RenderTreeBuilder builder, int index, object dataContext,
               PropertyInfo propInfoValue)
           {
               // the builder position is between the builder.OpenComponent() and builder.CloseComponent()
               // This means that the component of InputSelect is added en stil open for changes.
               // We can create a new RenderFragment and set the ChildContent attribute of the InputSelect component
               builder.AddAttribute(index + 1, nameof(InputSelect<TValue>.ChildContent),
                   new RenderFragment(builder =>
                   {
                       // check if the type of the propery is an Enum
                       if (typeof(TValue).IsEnum)
                       {
                           // when type is a enum present them as an <option> element 
                           // by leveraging the component InputSelectOption
                           var values = typeof(TValue).GetEnumValues();
                           foreach (var val in values)
                           {
                               //  Open the InputSelectOption component
                               builder.OpenComponent(0, typeof(InputSelectOption<string>));
   
                               // Set the value of the enum as a value and key parameter
                               builder.AddAttribute(1, nameof(InputSelectOption<string>.Value), val.ToString());
                               builder.AddAttribute(2, nameof(InputSelectOption<string>.Key), val.ToString());
   
                               // Close the component
                               builder.CloseComponent();
                           }
                       }
   
   
                   }));
   
           }
   
       }
   }
   
   ````

7. The last thing is to set it up in the Form component mapping in the `startup.cs` The mapping is done as a string because it could be used in a different way than `Type` to `Type` mapping.

8. ````c#
   public void ConfigureServices(IServiceCollection services)
           {           
   			services.AddSingleton(new FormGeneratorComponentsRepository(
                     new Dictionary<string, Type>()
                     {
                           {typeof(string).ToString(), typeof(InputText) },
                           {typeof(DateTime).ToString(), typeof(InputDate<>) },
                           {typeof(bool).ToString(), typeof(InputCheckbox) },
                           {typeof(FoodKind).ToString(), typeof(RenderChildrenInputSelect) },
                           {typeof(decimal).ToString(), typeof(InputNumber<>) }
                     }, null));
           }
   ````




Run the application and you should now have a generated form based on the POCO with working selection element! 🎉✨

   ![Working Form!!!](Build a form-generator in Blazor.assets/image-20200706170010351.png)



Now we need to add styling.... unfortunately this article is getting too long if we handle that part as well. This is just a proof of concept, I would like to make some improvements to make it more production ready:

- Move to separate project
- Implement the styling for frameworks like Bootstrap or your own CSS styling
- `ValidationMessage` in the element
- Translatable `ValidationMessage`
- Passthrough of the `EditForm` events

I'll will add the part two link when it's finished. Any feedback is welcome.

> working example is found here: https://github.com/Aaltuj/BlazorFormGeneratorDemo
