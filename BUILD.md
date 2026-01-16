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
