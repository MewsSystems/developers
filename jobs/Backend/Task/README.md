# Exchange Rate API

A REST API service that provides daily currency exchange rates from the Czech National Bank. Built with clean architecture principles, the service fetches, caches, and serves exchange rate data with support for filtering by currency and date.

**Purpose:** Simplify currency conversion by providing a reliable, fast, and well-documented API for accessing real-time and historical exchange rates against the Czech Koruna (CZK).

## Tech Stack

- **.NET 10.0** - Latest cross-platform framework
- **ASP.NET Core** - Web API
- **DataAnnotation** - Request validation
- **Microsoft.Extensions.Http.Resilience** - HTTP resilience (retry, circuit breaker, timeout)
- **Scalar** - Interactive API documentation
- **xUnit v3** - Unit testing
- **NSubstitute** - Mocking

## Quick Start

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Run the API

```bash
cd jobs/Backend/Task
dotnet run --project src/ExchangeRateUpdater.Api
```

Access the API at:
- **API Docs**: http://localhost:5213/scalar/v1
- **Swagger**: http://localhost:5213/openapi/v1.json

### Run Tests

```bash
dotnet test
```

All tests should pass.

### Response Format

**Success (200 OK):**
```json
{
  "data": [
    {
      "sourceCurrency": "USD",
      "targetCurrency": "CZK",
      "rate": 23.45
    }
  ]
}
```

**Error (400/404/502):**
```json
{
  "errorCode": "ExchangeRate.CurrencyNotFound",
  "errorMessage": "Exchange rate for currency 'XYZ' was not found in the source data."
}
```

## Architecture

Clean architecture with 4 layers:

```
Domain/              # Entities, value objects (Currency, ExchangeRate)
Application/         # Business logic, caching decorator
Infrastructure/      # External APIs, HTTP clients, CNB provider
Presentation/        # Controllers, validation, API responses
```

## Adding More Providers

To add a new exchange rate provider (e.g., ECB, Bank of England):

1. **Create provider folder:** `Infrastructure/Providers/Ecb/`
2. **Implement HTTP client:** Create `EcbApiClient` with provider-specific logic
3. **Create adapter:** Implement `IExternalExchangeRateClient` to map to common format
4. **Register in DI:** Add to `Infrastructure/Extensions/ServiceCollectionExtensions.cs`
5. **Configure:** Add provider settings to `appsettings.json`

**Run with hot reload:**
```bash
dotnet watch --project src/ExchangeRateUpdater.Api
```

**Test with coverage:**
```bash
dotnet test --collect:"XPlat Code Coverage"
```
