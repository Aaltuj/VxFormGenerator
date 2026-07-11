# Contributing to VxFormGenerator

Thanks for helping improve VxFormGenerator.

## Before You Open An Issue

Use the right channel so maintainers can respond with less back-and-forth:

- Bugs: open a GitHub issue with a minimal reproduction, affected package, and expected versus actual behavior.
- Feature ideas: open a GitHub issue focused on the user problem, not just the proposed API shape.
- Support questions: use the Discord server linked in the [README](README.md#contact). GitHub issues should stay focused on confirmed bugs and scoped feature requests.
- Security reports: follow [SECURITY.md](SECURITY.md). Do not post vulnerabilities in public issues or chat.

## Before You Open A Pull Request

Please:

1. Search for existing issues or pull requests first.
2. Keep changes scoped to one problem.
3. Update docs when behavior or public APIs change.
4. Add or update tests when behavior changes.

## Development Notes

This repository currently targets `net10.0`.

In the provided devcontainer or similar containerized environments, use the local SDK directly:

```bash
DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 /workspace/.dotnet/dotnet build VxFormGenerator.Core/VxFormGenerator.Core.csproj -m:1
DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 /workspace/.dotnet/dotnet test VxFormGenerator.Core.Tests/VxFormGenerator.Core.Tests.csproj -m:1
```

Prefer the metadata-first dynamic form path for new dynamic-form work. Runtime CLR type generation remains a server-side opt-in path, not the default contribution direction.

## Pull Request Checklist

- Explain the problem and the user-facing impact.
- Link the related issue when one exists.
- Note any breaking changes or migration concerns.
- Include screenshots or sample output when UI behavior changed.

## Review Expectations

Maintainers may ask contributors to reduce scope, split mixed changes, or add tests before review. That is intentional: small, reproducible changes are easier to validate and ship.
