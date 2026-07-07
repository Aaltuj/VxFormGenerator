# VxFormGenerator Visual Studio Extension

This is a Visual Studio VSIX MVP for lazy scaffolding of editable Razor form components.

## Command

After installing the VSIX, use:

`Tools` > `VxFormGenerator: Scaffold Razor Form...`

The command prompts for:

- compiled model assembly (`.dll`)
- full model type name
- target `.razor` file

It runs the same `VxFormGenerator.Tools` CLI used by the VS Code extension.

## Build Notes

This project targets Visual Studio 2022 and the Visual Studio SDK. Build it on Windows with Visual Studio installed.

The VSIX project is intentionally not included in the main cross-platform solution build because the Visual Studio SDK is Windows/Visual Studio-specific.

## Tests

The command-building helper is covered by a cross-platform test project. Run it locally with:

```bash
dotnet test VxFormGenerator.VisualStudio.Tests/VxFormGenerator.VisualStudio.Tests.csproj -m:1
```

Run the same tests in a container with:

```bash
docker build -f Dockerfile.test -t vxformgenerator-visualstudio-extension-test .
docker run --rm vxformgenerator-visualstudio-extension-test
```
