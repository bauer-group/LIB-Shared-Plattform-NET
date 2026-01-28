# BAUER GROUP Shared Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-512BD4)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/badge/NuGet-Ready-004880)](https://www.nuget.org/)

A comprehensive multi-target .NET shared library platform providing essential building blocks for enterprise applications within the BAUER GROUP ecosystem. Supports .NET 10, .NET 8, and .NET Standard 2.0 for maximum compatibility.

---

## Overview

The BAUER GROUP Shared Platform is a modular, multi-project solution designed to accelerate development across the organization. It provides battle-tested implementations for common enterprise requirements including logging, data persistence, API integrations, cloud services, and desktop UI components.

### Key Features

- **Multi-Target Support**: .NET 10, .NET 8, and .NET Standard 2.0 for broad compatibility
- **Modular Architecture**: Pick only the packages you need
- **Enterprise-Ready Logging**: NLog-based logging with Sentry integration for error tracking
- **Data Layer**: Support for SQLite (encrypted), LiteDB, and in-memory databases
- **API Integrations**: Pre-built API integrations for generic use
- **Cloud Services**: Cloudinary integration for media management
- **Desktop Components**: WPF/WinForms utilities with embedded Chromium browser support
- **Reporting**: Stimulsoft Reports integration for professional reporting

---

## Packages

| Package | Target Frameworks | Description |
|---------|-------------------|-------------|
| `BAUERGROUP.Shared.Core` | net10.0, net8.0, netstandard2.0 | Core utilities, extensions, logging (NLog + Sentry), error tracking |
| `BAUERGROUP.Shared.Data` | net10.0, net8.0, netstandard2.0 | Data persistence: SQLite, LiteDB, in-memory database, caching |
| `BAUERGROUP.Shared.API` | net10.0, net8.0, netstandard2.0 | Generic API integrations |
| `BAUERGROUP.Shared.API.Shipping` | net10.0, net8.0, netstandard2.0 | Shipping providers: DHL, DPD, GLS, Deutsche Post, UPS |
| `BAUERGROUP.Shared.Cloud` | net10.0, net8.0 | Cloud services: Cloudinary media management |
| `BAUERGROUP.Shared.Desktop` | net10.0-windows, net8.0-windows | WPF/WinForms utilities, behaviors, converters |
| `BAUERGROUP.Shared.Desktop.Browser` | net10.0-windows, net8.0-windows | Embedded Chromium browser (CefSharp) for WPF |
| `BAUERGROUP.Shared.Desktop.Reporting` | net10.0-windows, net8.0-windows | Stimulsoft Reports integration* |

*\*Requires separate Stimulsoft license*

---

## Installation

### Via NuGet Package Manager

```powershell
# Core package (required)
Install-Package BAUERGROUP.Shared.Core

# Optional packages
Install-Package BAUERGROUP.Shared.Data
Install-Package BAUERGROUP.Shared.API
Install-Package BAUERGROUP.Shared.API.Shipping
Install-Package BAUERGROUP.Shared.Cloud
Install-Package BAUERGROUP.Shared.Desktop
Install-Package BAUERGROUP.Shared.Desktop.Browser
Install-Package BAUERGROUP.Shared.Desktop.Reporting
```

### Via .NET CLI

```bash
dotnet add package BAUERGROUP.Shared.Core
```

---

## Quick Start

### Logging with BGLogger

```csharp
using BAUERGROUP.Shared.Core.Logging;

// Configure logging
var config = new BGLoggerConfiguration
{
    ApplicationName = "MyApplication",
    LogDirectory = @"C:\Logs",
    SentryDsn = "https://your-sentry-dsn@sentry.io/project",
    SentryEnvironment = "production"
};

BGLogger.Configure(config);
BGLogger.Reload();

// Use the logger
BGLogger.Info("Application started");
BGLogger.Error(exception, "An error occurred");
```

### Data Persistence with SQLite

```csharp
using BAUERGROUP.Shared.Data.Connection;

// Open encrypted SQLite database
var connection = SQLiteConnectionFactory.CreateConnection(
    "mydata.db",
    "encryption-key"
);

// Use with your preferred ORM or raw SQL
```

### Shopify Integration

```csharp
using BAUERGROUP.Shared.API.Shopify;

var client = new ShopifyClient(
    shopUrl: "mystore.myshopify.com",
    accessToken: "your-access-token"
);

var orders = await client.GetOrdersAsync();
```

### Embedded Browser (WPF)

```csharp
using BAUERGROUP.Shared.Desktop.Browser;

// Show embedded Chrome browser window
WPFToolboxBrowser.ChromeEmbeddedWebbrowserWindow(
    title: "Web View",
    url: "https://example.com",
    owner: this
);
```

---

## Requirements

### Runtime Requirements

- **.NET 10.0**, **.NET 8.0**, or **.NET Standard 2.0** compatible runtime
- **Windows** (for Desktop packages: `Desktop`, `Desktop.Browser`, `Desktop.Reporting`)

### Optional Requirements

- **Stimulsoft License** - Required for `BAUERGROUP.Shared.Desktop.Reporting`
- **Sentry Account** - For error tracking integration

---

## Project Structure

```
BAUERGROUP.Shared.Plattform/
├── src/
│   ├── BAUERGROUP.Shared.Core/                 # Core utilities & logging
│   ├── BAUERGROUP.Shared.Data/                 # Data persistence layer
│   ├── BAUERGROUP.Shared.API/                  # Generic API integrations
│   ├── BAUERGROUP.Shared.Cloud/                # Cloud service integrations
│   ├── BAUERGROUP.Shared.Desktop/              # WPF/WinForms utilities
│   ├── BAUERGROUP.Shared.Desktop.Browser/      # Embedded browser
│   └── BAUERGROUP.Shared.Desktop.Reporting/    # Reporting components
├── tests/
│   └── BAUERGROUP.Shared.Tests/          # Unit tests
├── assets/
│   └── icons/                            # Shared icons
├── docs/
│   ├── BUILD.md                          # Build documentation
│   ├── CHANGELOG.md                      # Version history
│   ├── DEPENDENCY-LICENSES.md            # License analysis
│   └── INSTALLATION.md                   # Installation guide
├── Directory.Build.props                 # Shared build configuration
├── Directory.Packages.props              # Central package management
└── BAUERGROUP.Shared.Plattform.sln      # Solution file
```

---

## Building from Source

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (includes support for .NET 8 and earlier)
- Visual Studio 2022 (17.12+) or JetBrains Rider 2024.3+

### Build

```bash
# Clone the repository
git clone https://github.com/bauergroup/BAUERGROUP.Shared.Plattform.git
cd BAUERGROUP.Shared.Plattform

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test
```

### Creating NuGet Packages

```bash
dotnet pack --configuration Release
```

Packages will be output to `bin/Release/*.nupkg`

---

## Configuration

### NLog Configuration

The library uses NLog for logging. Configuration can be done programmatically via `BGLoggerConfiguration` or through `nlog.config`.

### Sentry Integration

Two Sentry integration options are available:

1. **Official Sentry.NLog** (Recommended for BGLogger)
   - Automatic integration with NLog pipeline
   - Breadcrumbs, context, and event capture

2. **BGErrorTracking** (Standalone)
   - Independent Sentry wrapper
   - Direct API access for custom scenarios

---

## Third-Party Licenses

This project uses various open-source packages. See [DEPENDENCY-LICENSES.md](docs/DEPENDENCY-LICENSES.md) for a complete license analysis.

### Key Dependencies

| Package | License | Notes |
|---------|---------|-------|
| NLog | BSD 3-Clause | Logging framework |
| CefSharp | BSD 3-Clause | Chromium browser |
| Sentry | MIT | Error tracking |
| Stimulsoft | Proprietary | Requires separate license |

---

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Style

- Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful names for variables, methods, and classes
- Write XML documentation for public APIs
- Include unit tests for new functionality

---

## Support

For questions or issues:

- **Internal**: Contact BAUER GROUP Development Team
- **GitHub Issues**: [Report an issue](https://github.com/bauergroup/BAUERGROUP.Shared.Plattform/issues)

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

**Note**: Some dependencies (Stimulsoft) require separate commercial licenses.
See [DEPENDENCY-LICENSES.md](docs/DEPENDENCY-LICENSES.md) for details.

---

**BAUER GROUP** - *Building Better Software Together*
