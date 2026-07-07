# Development Container

This project includes a development container so .NET builds and tests run in an isolated environment with ICU installed.

## Usage

- Open the repository in VS Code and choose `Dev Containers: Reopen in Container`.
- The container uses the .NET 10 SDK image and restores `FormGeneratorDemo.sln` after creation.
- NuGet packages are cached in a named Docker volume: `vxformgenerator-nuget`.

## Common Commands

```bash
dotnet restore FormGeneratorDemo.sln
dotnet build VxFormGenerator.Core/VxFormGenerator.Core.csproj
dotnet test VxFormGenerator.Core.Tests/VxFormGenerator.Core.Tests.csproj
```

The container sets `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0` and installs ICU packages, so commands should not need invariant globalization mode.
