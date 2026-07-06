# VxFormGenerator Agent

You work on the VxFormGenerator Blazor/.NET solution.

## Project Context

- Repository path: `/workspace/VxFormGenerator`
- Solution: `FormGeneratorDemo.sln`
- Current branch is expected to be `master`.
- The local `master` branch may be ahead of `origin/master` with recent modernization commits.
- Keep the working tree clean and do not revert unrelated user changes.

## Main Projects

- `VxFormGenerator.Core`: core form generation, metadata rendering, layout, validation, runtime/source generation.
- `VxFormGenerator.Components.Plain`: unstyled component renderer package.
- `VxFormGenerator.Components.Bootstrap`: Bootstrap component renderer package.
- `VxFormGeneratorDemo.Shared`: shared demo pages, including `/dynamic-form`.
- `VxFormGeneratorDemo.Server`: Blazor server demo.
- `VxFormGeneratorDemo.Wasm`: Blazor WebAssembly demo.
- `VxFormGeneratorDemoData`: demo models/data.
- `VxFormGenerator.Core.Tests`: xUnit/bUnit tests.

## Current Direction

- Recent work modernized dynamic form generation for `net10.0`.
- The preferred dynamic path is metadata rendering with `VxFormMetadataBuilder` and `RenderVxFormMetadata`.
- Avoid using `Reflection.Emit` for the default dynamic form path because it is not portable to Blazor WebAssembly, AOT, or restricted runtimes.
- Runtime CLR type generation remains available through `VxFormRuntimeModelBuilder` for server-side scenarios that explicitly need reflected attributes.
- Source text generation is available through `VxFormModelSourceGenerator` for diagnostics, persistence, or build-time generation.

## Build And Test Notes

- `dotnet` may not be on `PATH` in this workspace.
- Use the local SDK directly: `/workspace/.dotnet/dotnet`.
- The Alpine/container environment may lack ICU. If the SDK fails with missing ICU, run commands with `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1`.
- Verified command for core build:
  - `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 /workspace/.dotnet/dotnet build VxFormGenerator.Core/VxFormGenerator.Core.csproj --no-restore -m:1`
- Full solution tests/builds may hang in this environment; isolate by project before assuming a code failure.

## Useful Commands

- Git status: `git status --short --branch`
- Recent history: `git log --oneline --decorate --graph -40`
- Core build: `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 /workspace/.dotnet/dotnet build VxFormGenerator.Core/VxFormGenerator.Core.csproj -m:1`
- Core tests: `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 /workspace/.dotnet/dotnet test VxFormGenerator.Core.Tests/VxFormGenerator.Core.Tests.csproj -m:1`

## Coding Rules

- Prefer small, targeted changes.
- Preserve the metadata-first dynamic form architecture.
- Add tests for behavior changes, especially in metadata builder/rendering, value conversion, layout/order, lookup options, nullable values, and visibility rules.
- Keep public API changes deliberate and documented in `README.md` when relevant.
- Do not introduce backward compatibility shims unless there is a concrete persisted-data, shipped-behavior, or external-consumer need.
- Do not commit unless explicitly requested.

## Known Follow-Ups

- Investigate environment-specific hangs in full solution test/build commands.
- Review package metadata/readme warnings.
- Consider version bumping from `0.4.0` before any release of the `net10.0` modernization.
- Review TODO/NotImplemented areas in `FormElementBase.cs` and `IRenderChildren.cs`.
