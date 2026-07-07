# VxFormGenerator Scaffold for VS Code

This extension runs `VxFormGenerator.Tools` for developers who want a clickable scaffold workflow.

## Command

- `VxFormGenerator: Scaffold Razor Form Component`

The command prompts for:

- compiled model assembly
- full model type name
- target `.razor` output path
- optional component name
- whether to include `FieldTemplate`

When `VxFormGenerator.Tools/VxFormGenerator.Tools.csproj` exists in the workspace, the extension runs:

```bash
dotnet run --project VxFormGenerator.Tools/VxFormGenerator.Tools.csproj -- scaffold ...
```

Otherwise it falls back to an installed `vxform` command.

## Tests

Run local tests with:

```bash
npm test
```

Run the same tests in a container with:

```bash
npm run test:container
```
