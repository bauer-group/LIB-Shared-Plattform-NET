# Semantic Release Konfiguration für .NET Projekte

Diese Dokumentation beschreibt die Semantic Release Konfiguration für automatische Versionierung und Release-Erstellung.

## Übersicht

[Semantic Release](https://semantic-release.gitbook.io/) automatisiert den gesamten Release-Prozess basierend auf [Conventional Commits](https://www.conventionalcommits.org/).

### Versionsschema

| Commit-Prefix | Bedeutung | Version-Bump |
|---------------|-----------|--------------|
| `feat:` | Neues Feature | Minor (1.0.0 → 1.1.0) |
| `fix:` | Bugfix | Patch (1.0.0 → 1.0.1) |
| `feat!:` oder `BREAKING CHANGE:` | Breaking Change | Major (1.0.0 → 2.0.0) |
| `docs:`, `chore:`, `style:`, `refactor:`, `test:` | Keine Änderung | Kein Release |

## Plugin-Konfiguration

### Aktuelle Konfiguration (`semantic-release.json`)

```json
{
  "branches": ["main"],
  "plugins": [
    "@semantic-release/commit-analyzer",
    "@semantic-release/release-notes-generator",
    ["@semantic-release/changelog", {
      "changelogFile": "CHANGELOG.md"
    }],
    ["semantic-release-dotnet", {
      "paths": ["Directory.Build.props"]
    }],
    ["@semantic-release/git", {
      "assets": ["CHANGELOG.md", "Directory.Build.props"],
      "message": "chore(release): ${nextRelease.version} [automated]\n\n${nextRelease.notes}"
    }],
    "@semantic-release/github"
  ]
}
```

### Plugin-Lifecycle

| Phase | Plugin | Beschreibung |
|-------|--------|--------------|
| **verifyConditions** | Alle | Prüft Voraussetzungen (Git, GitHub Token, etc.) |
| **analyzeCommits** | `commit-analyzer` | Analysiert Commits, bestimmt Version-Bump |
| **generateNotes** | `release-notes-generator` | Erstellt Release Notes aus Commits |
| **prepare** | `changelog` | Aktualisiert CHANGELOG.md |
| **prepare** | `semantic-release-dotnet` | Aktualisiert Version in .NET Dateien |
| **prepare** | `git` | Committed Änderungen ins Repository |
| **publish** | `github` | Erstellt GitHub Release mit Tag |

## Konfigurationsbeispiele

### 1. Zentrale Versionierung (empfohlen)

Für Projekte mit `Directory.Build.props` als zentraler Versionsverwaltung:

```json
["semantic-release-dotnet", {
  "paths": ["Directory.Build.props"]
}]
```

**Directory.Build.props:**
```xml
<PropertyGroup>
  <VersionPrefix>1.0.0</VersionPrefix>
</PropertyGroup>
```

### 2. Einzelne Projekte

Für Projekte ohne zentrale `Directory.Build.props`:

```json
["semantic-release-dotnet", {
  "paths": [
    "src/MyProject/MyProject.csproj"
  ]
}]
```

### 3. Mehrere unabhängige Projekte

```json
["semantic-release-dotnet", {
  "paths": [
    "src/ProjectA/ProjectA.csproj",
    "src/ProjectB/ProjectB.csproj",
    "src/ProjectC/ProjectC.csproj"
  ]
}]
```

### 4. Glob-Pattern

Alle `.csproj` Dateien automatisch finden:

```json
["semantic-release-dotnet", {
  "paths": ["src/**/*.csproj"]
}]
```

### 5. Gemischter Ansatz

Zentrale Props + einzelne Projekte:

```json
["semantic-release-dotnet", {
  "paths": [
    "Directory.Build.props",
    "tools/StandaloneApp/StandaloneApp.csproj"
  ]
}]
```

## Git-Plugin Konfiguration

Das `@semantic-release/git` Plugin muss alle geänderten Dateien kennen:

```json
["@semantic-release/git", {
  "assets": [
    "CHANGELOG.md",
    "Directory.Build.props"
  ],
  "message": "chore(release): ${nextRelease.version} [automated]\n\n${nextRelease.notes}"
}]
```

**Wichtig:** Bei Glob-Patterns oder mehreren Dateien:

```json
["@semantic-release/git", {
  "assets": [
    "CHANGELOG.md",
    "Directory.Build.props",
    "src/**/*.csproj"
  ],
  "message": "chore(release): ${nextRelease.version} [automated]\n\n${nextRelease.notes}"
}]
```

## Branches-Konfiguration

### Standard (nur main)

```json
{
  "branches": ["main"]
}
```

### Mit Pre-Releases

```json
{
  "branches": [
    "main",
    { "name": "develop", "prerelease": "beta" },
    { "name": "next", "prerelease": "rc" }
  ]
}
```

Ergibt Versionen wie:
- `main` → `1.2.3`
- `develop` → `1.2.4-beta.1`
- `next` → `1.2.4-rc.1`

## GitHub Actions Workflow

### Erforderliche Secrets

| Secret | Beschreibung |
|--------|--------------|
| `GITHUB_TOKEN` | Automatisch verfügbar, für GitHub Release |
| `NUGET_API_KEY` | Für NuGet.org Publishing (optional) |

### Erforderliche Permissions

```yaml
permissions:
  contents: write      # Für Git Push und Release
  issues: write        # Für Release Notes
  pull-requests: write # Für PR Comments
```

### Workflow-Integration

```yaml
release:
  name: 📦 Create Release
  uses: bauer-group/automation-templates/.github/workflows/modules-semantic-release.yml@main
  with:
    target-branch: 'main'
    force-release: false
  secrets: inherit
```

## Installation

### NPM Dependencies

Die Semantic Release Pipeline benötigt folgende npm-Pakete:

```bash
npm install -D semantic-release \
  @semantic-release/changelog \
  @semantic-release/git \
  @semantic-release/github \
  semantic-release-dotnet
```

### Lokaler Test

```bash
# Dry-Run (ohne tatsächliches Release)
npx semantic-release --dry-run

# Mit Debug-Output
DEBUG=semantic-release:* npx semantic-release --dry-run
```

## Troubleshooting

### Kein Release erstellt

1. **Commits prüfen:** Mindestens ein `feat:` oder `fix:` Commit seit letztem Tag
2. **Branch prüfen:** Muss in `branches` konfiguriert sein
3. **Token prüfen:** `GITHUB_TOKEN` muss write-Berechtigung haben

### Version nicht aktualisiert

1. **Pfad prüfen:** Datei muss in `paths` konfiguriert sein
2. **Format prüfen:** `<VersionPrefix>` oder `<Version>` Tag muss existieren
3. **Git assets prüfen:** Datei muss in `@semantic-release/git` assets sein

### CHANGELOG nicht aktualisiert

1. **Plugin-Reihenfolge:** `@semantic-release/changelog` muss vor `@semantic-release/git` kommen
2. **Assets prüfen:** `CHANGELOG.md` muss in git assets sein

## Referenzen

- [Semantic Release Dokumentation](https://semantic-release.gitbook.io/)
- [semantic-release-dotnet Plugin](https://github.com/nickolaj-jepsen/semantic-release-dotnet)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [bauer-group/automation-templates](https://github.com/bauer-group/automation-templates)
