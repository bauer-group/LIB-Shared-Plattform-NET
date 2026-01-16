# Build Documentation / Build-Dokumentation

This documentation describes how to build, test, and package the solution using the `dotnet` CLI.

Diese Dokumentation beschreibt die Nutzung der `dotnet` CLI zum Bauen, Testen und Packen der Solution.

---

## Prerequisites / Voraussetzungen

- .NET 10.0 SDK or higher / oder höher
- Optional: Visual Studio 2022 (17.x) or Visual Studio 2026 (18.x) / oder Visual Studio 2026 (18.x)

---

## Quick Start / Schnellstart

```bash
# Build the solution / Solution bauen
dotnet build

# Run tests / Tests ausführen
dotnet test

# Create NuGet packages / NuGet-Pakete erstellen
dotnet pack
```

---

## Commands in Detail / Befehle im Detail

### Build the Solution / Solution bauen

```bash
# Debug build (default) / Debug-Build (Standard)
dotnet build

# Release build / Release-Build
dotnet build -c Release

# Build a single project / Einzelnes Projekt bauen
dotnet build src/BAUERGROUP.Shared.Core/BAUERGROUP.Shared.Core.csproj
```

### Restore Dependencies / Abhängigkeiten wiederherstellen

```bash
# Restore NuGet packages / NuGet-Pakete wiederherstellen
dotnet restore
```

### Run Tests / Tests ausführen

```bash
# Run all tests / Alle Tests ausführen
dotnet test

# Tests with verbose output / Tests mit detaillierter Ausgabe
dotnet test -v normal

# Tests with code coverage / Tests mit Code-Coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test (filter by name) / Einzelnen Test ausführen (nach Name filtern)
dotnet test --filter "FullyQualifiedName~MyTestName"
```

### Create NuGet Packages / NuGet-Pakete erstellen

```bash
# Create debug packages / Debug-Pakete erstellen
dotnet pack

# Create release packages / Release-Pakete erstellen
dotnet pack -c Release

# Create packages in specific directory / Pakete in bestimmtes Verzeichnis erstellen
dotnet pack -c Release -o ./artifacts

# With specific version / Mit spezifischer Version
dotnet pack -c Release -p:VersionPrefix=1.2.3
```

### Clean Project / Projekt aufräumen

```bash
# Delete build artifacts / Build-Artefakte löschen
dotnet clean

# Delete all bin/obj folders (PowerShell) / Alle bin/obj Ordner löschen (PowerShell)
Get-ChildItem -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force
```

---

## Project Structure / Projektstruktur

| Project / Projekt | Description / Beschreibung |
| ----------------- | -------------------------- |
| `BAUERGROUP.Shared.Core` | Core functionality and utilities / Kernfunktionalitäten und Basis-Utilities |
| `BAUERGROUP.Shared.Data` | Data access and persistence / Datenzugriff und Persistenz |
| `BAUERGROUP.Shared.API` | HTTP/API client functionality / HTTP/API Client-Funktionalitäten |
| `BAUERGROUP.Shared.Cloud` | Cloud service integrations / Cloud-Service Integrationen |
| `BAUERGROUP.Shared.Desktop` | WPF/Desktop base components / WPF/Desktop Basis-Komponenten |
| `BAUERGROUP.Shared.Desktop.Browser` | Browser integration (CefSharp, WebView2) / Browser-Integration (CefSharp, WebView2) |
| `BAUERGROUP.Shared.Desktop.Reporting` | Reporting functionality (Stimulsoft) / Reporting-Funktionalitäten (Stimulsoft) |

---

## Configuration / Konfiguration

### Central Package Management

Package versions are managed centrally in [Directory.Packages.props](Directory.Packages.props).

Paketversionen werden zentral in [Directory.Packages.props](Directory.Packages.props) verwaltet.

### Build Properties / Build-Eigenschaften

Common build properties are defined in [Directory.Build.props](Directory.Build.props):

Gemeinsame Build-Eigenschaften sind in [Directory.Build.props](Directory.Build.props) definiert:

- Target Framework: `net10.0`
- Nullable Reference Types: enabled / aktiviert
- XML Documentation: enabled / aktiviert
- Assembly Signing: enabled / aktiviert

---

## CI/CD

Deterministic builds are automatically enabled when `GITHUB_ACTIONS=true` is set.

Für CI/CD-Builds werden deterministische Builds automatisch aktiviert wenn `GITHUB_ACTIONS=true` gesetzt ist.

```bash
# Simulate CI build / CI-Build simulieren
dotnet build -c Release -p:ContinuousIntegrationBuild=true
```

---

## GitHub Actions Workflow

The repository includes a comprehensive GitHub Actions workflow for automated CI/CD.

Das Repository enthält einen umfassenden GitHub Actions Workflow für automatisierte CI/CD.

### Features / Funktionen

| Feature | Description / Beschreibung |
| ------- | -------------------------- |
| **PR Validation** | Build and test on every pull request / Build und Test bei jedem Pull Request |
| **Semantic Versioning** | Automatic version bumps based on conventional commits / Automatische Versionierung basierend auf Conventional Commits |
| **Release Creation** | Automatic GitHub releases with changelog / Automatische GitHub Releases mit Changelog |
| **NuGet Publishing** | Publish to NuGet.org and GitHub Packages / Veröffentlichung auf NuGet.org und GitHub Packages |
| **Assembly Signing** | Strong-name signing with SNK key / Strong-Name Signierung mit SNK-Schlüssel |
| **Dependabot** | Automatic dependency updates / Automatische Dependency-Updates |

### Conventional Commits

Use conventional commit messages for automatic versioning:

Verwende Conventional Commit Messages für automatische Versionierung:

| Prefix | Version Bump | Example / Beispiel |
| ------ | ------------ | ------------------ |
| `feat:` | Minor (1.x.0) | `feat: add new helper method` |
| `fix:` | Patch (1.0.x) | `fix: correct null handling` |
| `feat!:` | Major (x.0.0) | `feat!: breaking API change` |
| `BREAKING CHANGE:` | Major (x.0.0) | `refactor: update API\n\nBREAKING CHANGE: method renamed` |

### Required Secrets / Erforderliche Secrets

Configure these secrets in your GitHub repository settings:

Diese Secrets müssen in den GitHub Repository-Einstellungen konfiguriert werden:

| Secret | Description / Beschreibung |
| ------ | -------------------------- |
| `NUGET_API_KEY` | API key for NuGet.org publishing / API-Schlüssel für NuGet.org Veröffentlichung |
| `DOTNET_SIGNKEY_BASE64` | Base64-encoded SNK file for assembly signing / Base64-kodierte SNK-Datei für Assembly-Signierung |

### Setup NuGet API Key / NuGet API-Schlüssel einrichten

1. Go to [NuGet.org API Keys](https://www.nuget.org/account/apikeys) / Zu NuGet.org API Keys gehen
2. Create new API key with push permissions / Neuen API-Schlüssel mit Push-Rechten erstellen
3. Add as GitHub Secret `NUGET_API_KEY` / Als GitHub Secret `NUGET_API_KEY` hinzufügen

### Setup Assembly Signing Key / SNK-Schlüssel einrichten

```bash
# Generate new SNK key / Neuen SNK-Schlüssel generieren
sn -k BAUERGROUP.Shared.snk

# Convert to Base64 for GitHub Secret / In Base64 für GitHub Secret konvertieren
# Linux/macOS:
base64 -w 0 BAUERGROUP.Shared.snk > key.b64

# Windows PowerShell:
[Convert]::ToBase64String([IO.File]::ReadAllBytes("BAUERGROUP.Shared.snk"))
```

### Manual Workflow Trigger / Manueller Workflow-Start

You can manually trigger the workflow with optional parameters:

Der Workflow kann manuell mit optionalen Parametern gestartet werden:

- **force-release**: Create release even without conventional commits / Release erstellen auch ohne Conventional Commits
- **version-override**: Specify exact version (e.g., `1.2.3`) / Exakte Version angeben (z.B. `1.2.3`)

---

## Visual Studio Code

VS Code configuration is included in the `.vscode` folder.

VS Code Konfiguration ist im `.vscode` Ordner enthalten.

### Keyboard Shortcuts / Tastaturkürzel

| Shortcut | Action / Aktion |
| -------- | --------------- |
| `Ctrl+Shift+B` | Build (default task) / Bauen (Standard-Task) |
| `Ctrl+Shift+T` | Run tests / Tests ausführen |
| `F5` | Start debugging / Debuggen starten |

### Recommended Extensions / Empfohlene Erweiterungen

Open VS Code and accept the recommended extensions, or run:

VS Code öffnen und empfohlene Erweiterungen akzeptieren, oder ausführen:

```bash
code --install-extension ms-dotnettools.csharp
code --install-extension ms-dotnettools.csdevkit
code --install-extension fernandoescolar.vscode-solution-explorer
code --install-extension ryanluker.vscode-coverage-gutters
```

### Available Tasks / Verfügbare Tasks

- `build` - Build solution (Debug) / Solution bauen (Debug)
- `build (Release)` - Build solution (Release) / Solution bauen (Release)
- `test` - Run all tests / Alle Tests ausführen
- `test (with coverage)` - Run tests with coverage / Tests mit Coverage ausführen
- `pack` - Create NuGet packages / NuGet-Pakete erstellen
- `clean` - Clean build artifacts / Build-Artefakte löschen
- `restore` - Restore dependencies / Abhängigkeiten wiederherstellen

---

## Troubleshooting / Fehlerbehebung

### Build fails / Build schlägt fehl

```bash
# Clear caches and rebuild / Caches leeren und neu bauen
dotnet nuget locals all --clear
dotnet restore --force
dotnet build
```

### Tests not found / Tests werden nicht gefunden

```bash
# Build test project directly / Test-Projekt direkt bauen
dotnet build tests/BAUERGROUP.Shared.Tests/BAUERGROUP.Shared.Tests.csproj
dotnet test tests/BAUERGROUP.Shared.Tests/BAUERGROUP.Shared.Tests.csproj -v detailed
```
