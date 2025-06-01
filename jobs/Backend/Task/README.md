# Exchange Rate Updater

A C# 13 console application targeting .NET 9 that fetches and displays exchange rates from the Czech National Bank (CNB) API. The solution follows clean architecture principles with dependency injection, caching, logging, and unit testing.

## Features

- Fetches exchange rates from Czech National Bank API
- Built-in caching to reduce API calls
- Comprehensive logging with Serilog
- HTTP retry policies with Polly
- Unit tests with xUnit, FakeItEasy, and FluentAssertions
- Dependency injection

## Project Structure

```
├── ExchangeRateUpdater/         # Console application
├── Services/                    # Core business logic
│   ├── CzechCrown/              # Czech National Bank implementation
│   └── Core models              # Currency, ExchangeRate, etc.
└── Services.Test/               # Unit tests
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio 2022 or VS Code

### Configuration

The application is configured via `appsettings.json`, or `appsettings.Development.json`.

### Running the Application

1. Clone the repository
2. Navigate to the project directory `./ExchangeRateUpdater`
3. Run the application with currency codes as arguments:

```bash
dotnet run USD EUR JPY GBP
```

Or use the provided launch settings in Visual Studio (`F5` or `Ctrl + F5`).

### Example Output

```
Successfully retrieved 4 exchange rates:
EUR/CZK=25.123
GBP/CZK=29.456
JPY/CZK=0.234
USD/CZK=23.789
```

## Architecture

### Core Components

- **[`IExchangeRateProvider`](Services/IExchangeRateProvider.cs)**: Main interface for fetching exchange rates
- **[`CzechCrownRateProvider`](Services/CzechCrown/CzechCrownRateProvider.cs)**: Implementation for Czech National Bank
- **[`CzechNationalBankCachedClient`](Services/CzechCrown/CzechNationalBankCachedClient.cs)**: Cached HTTP client implementation

### Key Features

#### Dependency Injection

Services are registered in [`ServiceCollectionExtension`](Services/ServiceCollectionExtension.cs):

- Configuration options with validation
- Distributed caching
- Logging with Serilog

#### Caching

The [`CzechNationalBankCachedClient`](Services/CzechCrown/CzechNationalBankCachedClient.cs) implements distributed caching to reduce API calls and improve performance.

#### Error Handling

- HTTP retry policies using Polly
- Handling of missing currencies
- Comprehensive logging for debugging

#### Data Models

- **[`Currency`](Services/Currency.cs)**: Represents a currency with ISO 4217 code
- **[`ExchangeRate`](Services/ExchangeRate.cs)**: Represents an exchange rate between two currencies
- **[`CzkExchangeRateResponse`](Services/CzechCrown/Models/CzkExchangeRateResponse.cs)**: API response models

## Testing

The solution includes a basic set of unit tests in the [`Services.Test`](Services.Test/) project.

### Running Tests

```bash
dotnet test
```

Or from Visual Studio - `Test Explorer` tab.

## Logging

The application uses Serilog for structured logging:

- **Development**: Console output with debug level
- **Production**: Configurable for Application Insights or other sinks

Logging configuration is in [`appsettings.Development.json`](ExchangeRateUpdater/appsettings.Development.json).

## Building

```bash
dotnet build
```

## API Information

This application uses the Czech National Bank API:

- **Base URL**: https://api.cnb.cz

## Contributing

1. Follow the existing code style (see [`.editorconfig`](.editorconfig))
2. Add unit tests for new features
3. Update documentation as needed
4. Ensure all tests pass

## License

This project is for demonstration purposes.
