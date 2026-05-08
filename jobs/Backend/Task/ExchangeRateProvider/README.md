# Exchange Rate Provider API

A REST API for retrieving exchange rates for a given currency with optional filtering, built with .NET 10, following clean architecture principles and SOLID design patterns.

## Architecture

The solution follows **Clean Architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│                 Presentation Layer                      │
│              ExchangeRateProvider.Api                   │
│        (Controllers, Validators, OpenAPI Config)        │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                Application Layer                        │
│           ExchangeRateProvider.Application              │
│          (Handlers, Queries, DTOs, CQRS)                │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                    Domain Layer                         │
│             ExchangeRateProvider.Domain                 │
│     (Entities, Value Objects, Interfaces, Constants)    │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                Infrastructure Layer                     │
│          ExchangeRateProvider.Infrastructure            │
│    (External Services, HTTP Clients, Polly Policies)    │
└─────────────────────────────────────────────────────────┘
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)

### Building the Solution

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build
```

### Running Locally

```bash
# Run the API
cd src/ExchangeRateProvider.Api
dotnet run

# API will be available at:
# - HTTP: http://localhost:5291
# - HTTPS: https://localhost:7291
```

### Running Tests

```bash
# Run all tests
dotnet test
```

## Docker

### Building the Docker Image

```bash
# Build from solution root
docker build -t exchange-rate-provider-api -f src/ExchangeRateProvider.Api/Dockerfile .
```

### Running the Container

```bash
# Run on port 8080
docker run -p 8080:8080 --name exchange-rate-api exchange-rate-provider-api
```

## API Documentation

### Accessing API Documentation

When running in **Development** mode, interactive API documentation is available via **Scalar**:

```
https://localhost:5291/scalar/v1
```

### API Endpoints

#### Get Exchange Rates

**Endpoint**: `GET v1/api/exchange-rates`

**Query Parameters**:
- `baseCurrency` (required): The base currency code (e.g., "CZK")
- `quoteCurrencies` (optional): List of quote currency codes to filter results

**Example Requests**:

```bash
# Get all available exchange rates for CZK
curl "http://localhost:8080/v1/api/exchangerates?baseCurrency=CZK"

# Get specific quote currencies
curl "http://localhost:8080/v1/api/exchangerates?baseCurrency=CZK&quoteCurrencies=EUR&quoteCurrencies=USD&quoteCurrencies=GBP"
```

**Example Response** (200 OK):

```json
[
  {
    "baseCurrency": {
      "code": "CZK"
    },
    "quoteCurrency": {
      "code": "EUR"
    },
    "rate": 0.04012
  },
  {
    "baseCurrency": {
      "code": "CZK"
    },
    "quoteCurrency": {
      "code": "USD"
    },
    "rate": 0.04357
  }
]
```

**Error Responses**:

- **400 Bad Request**: Invalid or unsupported currency
  ```json
  {
    "title": "Invalid base currency",
    "detail": "Base currency is required.",
    "status": 400
  }
  ```

- **400 Bad Request**: Unsupported currency
  ```json
  {
    "title": "Unsupported base currency",
    "detail": "The currency 'XXX' is not supported. Supported currencies: CZK",
    "status": 400
  }
  ```

### Supported Currencies

**Base Currency**: Currently supports `CZK` (Czech Koruna)

**Quote Currencies**: All currencies provided by the CNB API, typically including:
- EUR (Euro)
- USD (US Dollar)
- GBP (British Pound)
- JPY (Japanese Yen)
- And 30+ other major world currencies

The API returns only currencies provided by the source - no calculated inverse rates.

## Configuration

### Application Settings

Configuration is managed via `appsettings.json` and `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```