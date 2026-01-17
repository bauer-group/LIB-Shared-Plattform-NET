# Build Documentation

This documentation describes how to build, test, and package the solution using the `dotnet` CLI.

---

## Prerequisites

- .NET 10.0 SDK or higher
- Optional: Visual Studio 2022 (17.x) or Visual Studio 2026 (18.x)

---

## Quick Start

```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Create NuGet packages
dotnet pack
```

---

## Commands in Detail

### Build the Solution

```bash
# Debug build (default)
dotnet build

# Release build
dotnet build -c Release

# Build a single project
dotnet build src/BAUERGROUP.Shared.Core/BAUERGROUP.Shared.Core.csproj
```

### Restore Dependencies

```bash
# Restore NuGet packages
dotnet restore
```

### Run Tests

```bash
# Run all tests
dotnet test

# Tests with verbose output
dotnet test -v normal

# Tests with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test (filter by name)
dotnet test --filter "FullyQualifiedName~MyTestName"
```

### Create NuGet Packages

```bash
# Create debug packages
dotnet pack

# Create release packages
dotnet pack -c Release

# Create packages in specific directory
dotnet pack -c Release -o ./artifacts

# With specific version
dotnet pack -c Release -p:VersionPrefix=1.2.3
```

### Clean Project

```bash
# Delete build artifacts
dotnet clean

# Delete all bin/obj folders (PowerShell)
Get-ChildItem -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force
```

---

## Project Structure

| Project | Description |
|---------|-------------|
| `BAUERGROUP.Shared.Core` | Core functionality and utilities |
| `BAUERGROUP.Shared.Data` | Data access and persistence |
| `BAUERGROUP.Shared.API` | HTTP/API client functionality |
| `BAUERGROUP.Shared.Cloud` | Cloud service integrations |
| `BAUERGROUP.Shared.Desktop` | WPF/Desktop base components |
| `BAUERGROUP.Shared.Desktop.Browser` | Browser integration (CefSharp, WebView2) |
| `BAUERGROUP.Shared.Desktop.Reporting` | Reporting functionality (Stimulsoft) |

---

## Configuration

### Central Package Management

Package versions are managed centrally in [Directory.Packages.props](../Directory.Packages.props).

### Build Properties

Common build properties are defined in [Directory.Build.props](../Directory.Build.props):

- Target Framework: `net10.0`
- Nullable Reference Types: enabled
- XML Documentation: enabled
- Assembly Signing: enabled

---

## CI/CD

Deterministic builds are automatically enabled when `GITHUB_ACTIONS=true` is set.

```bash
# Simulate CI build
dotnet build -c Release -p:ContinuousIntegrationBuild=true
```

---

## GitHub Actions Workflow

The repository includes a comprehensive GitHub Actions workflow for automated CI/CD.

### Features

| Feature | Description |
|---------|-------------|
| **PR Validation** | Build and test on every pull request |
| **Semantic Versioning** | Automatic version bumps based on conventional commits |
| **Release Creation** | Automatic GitHub releases with changelog |
| **NuGet Publishing** | Publish to NuGet.org and GitHub Packages |
| **Assembly Signing** | Strong-name signing with SNK key |
| **Dependabot** | Automatic dependency updates |

### Conventional Commits

Use conventional commit messages for automatic versioning:

| Prefix | Version Bump | Example |
|--------|--------------|---------|
| `feat:` | Minor (1.x.0) | `feat: add new helper method` |
| `fix:` | Patch (1.0.x) | `fix: correct null handling` |
| `feat!:` | Major (x.0.0) | `feat!: breaking API change` |
| `BREAKING CHANGE:` | Major (x.0.0) | `refactor: update API\n\nBREAKING CHANGE: method renamed` |

### Required Secrets

Configure these secrets in your GitHub repository settings:

| Secret | Description |
|--------|-------------|
| `NUGET_API_KEY` | API key for NuGet.org publishing |
| `DOTNET_SIGNKEY_BASE64` | Base64-encoded SNK file for assembly signing |

### Setup NuGet API Key

1. Go to [NuGet.org API Keys](https://www.nuget.org/account/apikeys)
2. Create new API key with push permissions
3. Add as GitHub Secret `NUGET_API_KEY`

### Setup Assembly Signing Key

```bash
# Generate new SNK key
sn -k BAUERGROUP.Shared.snk

# Convert to Base64 for GitHub Secret
# Linux/macOS:
base64 -w 0 BAUERGROUP.Shared.snk > key.b64

# Windows PowerShell:
[Convert]::ToBase64String([IO.File]::ReadAllBytes("BAUERGROUP.Shared.snk"))
```

### Manual Workflow Trigger

You can manually trigger the workflow with optional parameters:

- **force-release**: Create release even without conventional commits
- **version-override**: Specify exact version (e.g., `1.2.3`)

---

## Visual Studio Code

VS Code configuration is included in the `.vscode` folder.

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+Shift+B` | Build (default task) |
| `Ctrl+Shift+T` | Run tests |
| `F5` | Start debugging |

### Recommended Extensions

Open VS Code and accept the recommended extensions, or run:

```bash
code --install-extension ms-dotnettools.csharp
code --install-extension ms-dotnettools.csdevkit
code --install-extension fernandoescolar.vscode-solution-explorer
code --install-extension ryanluker.vscode-coverage-gutters
```

### Available Tasks

- `build` - Build solution (Debug)
- `build (Release)` - Build solution (Release)
- `test` - Run all tests
- `test (with coverage)` - Run tests with coverage
- `pack` - Create NuGet packages
- `clean` - Clean build artifacts
- `restore` - Restore dependencies

---

## Troubleshooting

### Build Fails

```bash
# Clear caches and rebuild
dotnet nuget locals all --clear
dotnet restore --force
dotnet build
```

### Tests Not Found

```bash
# Build test project directly
dotnet build tests/BAUERGROUP.Shared.Tests/BAUERGROUP.Shared.Tests.csproj
dotnet test tests/BAUERGROUP.Shared.Tests/BAUERGROUP.Shared.Tests.csproj -v detailed
```
