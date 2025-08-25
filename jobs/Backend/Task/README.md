# Exchange Rates Example Solution

A .NET 9 multi-project solution demonstrating a clean, extensible approach to fetching and exposing foreign exchange rates with a modern service-oriented architecture.

## Projects

- **ExchangeRateApi** (ASP.NET Core Web API)
  - Exposes HTTP endpoints for requesting exchange rates and listing providers.
  - Uses FluentValidation for request validation and Swagger / Swashbuckle for OpenAPI docs.
  - Integrates with ExchangeRateProviders via `IExchangeRateService`.

- **ExchangeRateProviders** (Class Library)
  - Core service abstraction (`IExchangeRateService`) for unified exchange rate operations.
  - Data provider abstractions (`IExchangeRateDataProvider`, `IExchangeRateDataProviderFactory`) separating data acquisition from business logic.
  - Concrete CZK implementation (`CzkExchangeRateDataProvider`) backed by Czech National Bank API.
  - Service-oriented architecture with clear separation of concerns.
  - Intelligent caching with FusionCache and timezone-aware strategies.

- **ExchangeRateApi.Tests** (NUnit)
  - Unit tests for controller behavior, validation, logging, and endpoint responses.

- **ExchangeRateProviders.Tests** (NUnit)
  - Unit tests for service logic, provider resolution, filtering, logging, and edge cases.

- **ExchangeRateUpdater** (Console App)
  - Simple example consumer demonstrating `IExchangeRateService` usage.

## Architecture Overview

The solution follows a layered architecture with clear separation of concerns:

### ExchangeRateProviders Library Architecture

1. **Service Layer**: `ExchangeRateService` - Main business logic and entry point
2. **Factory Layer**: `ExchangeRateDataProviderFactory` - Resolves providers by target currency
3. **Provider Layer**: `IExchangeRateDataProvider` implementations - Handle data source-specific logic

### Key Interfaces
// Main service interface - primary entry point
public interface IExchangeRateService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string TargetCurrency, IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}

// Data provider abstraction
public interface IExchangeRateDataProvider
{
    string ExchangeRateProviderTargetCurrencyCode { get; }
    Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken);
}

// Factory for provider resolution
public interface IExchangeRateDataProviderFactory
{
    IExchangeRateDataProvider GetProvider(string exchangeRateProviderCurrencyCode);
}
## Get up and Running with Docker
docker compose -f docker-compose.dev.yml up -d --build
- The API will be available at `http://localhost:8080/swagger/index.html`
- Alternatively public access to the API via http://128.140.72.56:18080/swagger/index.html, deployed on hetzner cloud via github actions CI/CD pipeline.

## Endpoint Summary (v1)

All endpoints are versioned under `v1/api`.

- `POST v1/api/exchange-rates/rates` - Request rates via JSON body (list of source currency codes + optional target currency; defaults to CZK).
- `GET v1/api/exchange-rates/rates?currencies=USD,EUR&targetCurrency=CZK` - Query version.
- `GET v1/api/providers` - List available providers.

Swagger UI: `/swagger` (served once the API is running).

## API Documentation

You can access the interactive Swagger UI documentation at:
- **Swagger UI**: `/swagger`
- **OpenAPI JSON**: `/swagger/v1/swagger.json`

The Swagger UI provides:
- Interactive API testing
- Detailed endpoint documentation
- Request/response examples
- Schema definitions
- Authentication details (when applicable)

## Adding a New Provider

The new architecture makes adding providers straightforward:

1. **Implement `IExchangeRateDataProvider`** for your target currency:public class UsdExchangeRateDataProvider : IExchangeRateDataProvider
{
    public string ExchangeRateProviderTargetCurrencyCode => "USD";
    
    public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken)
    {
        // Fetch from Fed API, apply caching, map to ExchangeRate objects
       }
   }
2. **Register in DI container**:services.AddSingleton<IExchangeRateDataProvider, UsdExchangeRateDataProvider>();
3. **Use the service** - the factory automatically discovers providers by target currency:var rates = await exchangeRateService.GetExchangeRatesAsync("USD", currencies, cancellationToken);
## Configuration & Extensibility

- **Logging**: Console provider added by default; extend via `appsettings.json` or additional logging providers.
- **Caching**: FusionCache with intelligent caching strategies per provider (e.g., Prague timezone-aware for CNB).
- **Validation**: Add new validators implementing `IValidator<T>` and register in DI.
- **Service Registration**: Simple DI registration automatically wires up the service layer.

## Current Implementation: CZK Provider

- **Target Currency**: CZK (Czech Koruna)
- **Data Source**: Czech National Bank (CNB) HTTP API
- **Caching**: Prague timezone-aware cache expiration
- **Features**: 
  - Automatic rate normalization (handles multi-unit amounts like JPY/100)
  - Comprehensive error handling and logging
  - Resilient HTTP client with retry policies

## Architecture Benefits

- **Separation of Concerns**: Clear boundaries between service logic, provider resolution, and data acquisition
- **Extensibility**: Easy to add new target currencies without modifying existing code
- **Testability**: Each layer can be independently unit tested
- **Flexibility**: Target currency specified per request, supporting multi-currency scenarios
- **Observability**: Comprehensive structured logging throughout the call chain

## Future Enhancements

- Additional provider implementations (ECB for EUR, Federal Reserve for USD, etc.)
- Fallback rate sources (CSV, XML, database) for high availability
- Health checks for monitoring provider status
- Nuget package ExchangeRateProviders library
