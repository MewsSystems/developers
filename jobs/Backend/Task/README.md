# Exchange Rate Updater

A production-ready .NET 9 solution for fetching and managing exchange rates from the Czech National Bank, featuring both console and Web API interfaces.

## Architecture

This solution implements a clean, modular architecture with the following components:

### Projects

```
ExchangeRateUpdater.sln
├── ExchangeRateUpdater.Core/          # Shared business logic library
├── ExchangeRateUpdater.Console/       # Console application
├── ExchangeRateUpdater.Api/           # Web API project  
└── ExchangeRateUpdater.Tests/         # Unit and integration tests
```

### Core Library (`ExchangeRateUpdater.Core`)

Contains all shared business logic, models, and interfaces:

- **Models**: `Currency`, `ExchangeRate`, DTOs for API responses
- **Interfaces**: `IExchangeRateProvider`, `IExchangeRateCache`, `ILogger`
- **Services**: `ExchangeRateService`, `InMemoryExchangeRateCache`
- **Providers**: `CzechNationalBankProvider`
- **Configuration**: Options classes for dependency injection

### Console Application (`ExchangeRateUpdater.Console`)

A command-line interface for fetching exchange rates:
- No caching (as per requirements)
- Uses `System.CommandLine` for argument parsing
- References the Core library

### Web API (`ExchangeRateUpdater.Api`)

A REST API with the following features:
- OpenAPI/Swagger documentation
- Enhanced caching strategy (caches all rates per date)
- Follows REST API best practices
- Memory-based caching using `IMemoryCache`

### Testing (`ExchangeRateUpdater.Tests`)

Comprehensive test suite covering:
- Unit tests for core components
- Integration tests for the API
- Mock-based testing using Moq

## Key Features

- **Clean Architecture**: Separation of concerns with Core library
- **Dependency Injection**: Full DI container setup across all projects
- **Error Handling**: Comprehensive error handling with retry policies using Polly
- **Caching**: Different caching strategies for Console (none) and API (enhanced)
- **Configuration**: JSON-based configuration with environment variable overrides
- **API Documentation**: OpenAPI/Swagger integration
- **Testing**: Unit and integration tests

### Running the Console Application

```bash
cd ExchangeRateUpdater.Console

# Basic usage (defaults to today's date and predefined currencies)
dotnet run

# Specify a custom date
dotnet run -- --date 2025-09-20

# Specify custom currencies
dotnet run -- --currencies USD,EUR,JPY

# Combine both parameters
dotnet run -- --date 2025-09-20 --currencies USD,EUR,JPY

# View help information
dotnet run -- --help
```

### Running the API

```bash
cd ExchangeRateUpdater.Api
dotnet run
```

The API will be available at `https://localhost:5001` (or the port shown in the console).
OpenAPI document is available at `/openapi/v1.json` when running in Development mode.

### API Endpoints

- `GET /api/exchangerates?currencies=USD,EUR&date=2025-09-20` - Get exchange rates

### Running Tests

```bash
cd ExchangeRateUpdater.Tests
dotnet test
```

## Configuration

The application uses `appsettings.json` for configuration:

```json
{
  "ExchangeRate": {
    "DefaultCacheExpiry": "01:00:00",
    "MaxRetryAttempts": 3,
    "RetryDelay": "00:00:02",
    "RequestTimeout": "00:00:30",
    "EnableCaching": true
  },
  "CzechNationalBank": {
    "BaseUrl": "https://api.cnb.cz/cnbapi/exrates/daily",
    "DateFormat": "yyyy-MM-dd",
    "Language": "EN"
  }
}
```

## Caching Strategy

### Console Application
- **No caching** (as per requirements)
- Direct calls to the provider

### API Application
- **Enhanced caching**: All exchange rates are cached per date
- When requesting specific currencies, the API returns filtered results from the cached data
- Uses `IMemoryCache` for efficient in-memory storage
- Configurable cache expiry times

## API Design

The API follows REST best practices:

- **Resource-based URLs**: `/api/exchangerates`
- **HTTP verbs**: GET for queries
- **Status codes**: Proper HTTP status codes (200, 400, 404, 500)
- **Content negotiation**: JSON responses
- **OpenAPI documentation**: Complete API documentation with Swagger
