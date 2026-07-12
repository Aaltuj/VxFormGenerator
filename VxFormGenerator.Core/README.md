# VxFormGenerator - Easy form creation by Model or Dynamic approach

This library is sprout of a POC I've created for an [article](https://github.com/Aaltuj/VxFormGenerator). Based on that work and the improvements made in this [article](https://medium.com/@aaltuj/simple-to-use-dynamic-form-generator-powered-by-blazor-c38a068576e7), I decided to publish it a as [NuGet](https://www.nuget.org/packages/VxFormGenerator/0.0.3) package.

> Be aware that the current state is not ready for production, use it AT YOUR OWN RISK. Also to fully style all the built-in Blazor components it's recommended that you wait for [this pull request](https://github.com/dotnet/aspnetcore/pull/24835) for the `aspnetcore` repo to be merged and released. Allowing use to implement custom Validation classes. The current state of the library has a ugly workaround.
### UI component packages

Choose one renderer package for the CSS framework used by the host app:

```bash
dotnet add package VxFormGenerator.Components.Plain
dotnet add package VxFormGenerator.Components.Bootstrap
dotnet add package VxFormGenerator.Components.Bulma
dotnet add package VxFormGenerator.Components.Tailwind
```

The styled packages add framework-specific classes to generated fields. The host app remains responsible for loading Bootstrap, Bulma, or Tailwind CSS. Register the matching namespace before calling `AddVxFormGenerator`, for example `VxFormGenerator.Settings.Bulma` or `VxFormGenerator.Settings.Tailwind`.

For Tailwind, include the renderer package and your app markup in the Tailwind content scan or safelist the renderer utilities. The demo uses a checked-in prebuilt stylesheet at `_content/VxFormGeneratorDemo.Shared/css/tailwind/vxform-tailwind.css`; production apps should generate an equivalent stylesheet from their Tailwind build.


### Usage

To 