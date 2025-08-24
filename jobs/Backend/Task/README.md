# Exchange Rates Example Solution

A .NET 9 multi-project solution demonstrating a clean, extensible approach to fetching and exposing foreign exchange rates.

## Projects

- ExchangeRateApi (ASP.NET Core Web API)
  - Exposes HTTP endpoints for requesting exchange rates and listing providers.
  - Uses FluentValidation for request validation and Swagger / Swashbuckle for OpenAPI docs.
- ExchangeRateProviders (Class Library)
  - Core abstractions (IExchangeRateProvider, IExchangeRateDataProvider, IExchangeRateProviderFactory).
  - Concrete CZK provider backed by the Czech National Bank feed.
  - Designed for easy extension with additional base currency providers.
- ExchangeRateApi.Tests (NUnit)
  - Unit tests for controller behavior, validation, logging, and endpoint responses.
- ExchangeRateProviders.Tests (NUnit)
  - Unit tests for provider filtering, logging, and edge cases.
- ExchangeRateUpdater (Console App)
  - Simple example consumer that resolves a provider and prints exchange rates to the console.

## Get up and Running with docker

```
docker compose -f docker-compose.yml -f docker-compose.dev.yml up -d --build 
```

- The API will be available at `http://localhost:8080/swagger/index.html`

- Alternatively public access to the API via http://128.140.72.56:18080/swagger/index.html, deployed on hetzner cloud via github actions CI/CD pipeline.


## Endpoint Summary (v1)
All endpoints are versioned under `v1/api`.

- `POST v1/api/exchange-rates/rates` Request rates via JSON body (list of source currency codes + optional target currency; defaults to CZK).
- `GET  v1/api/exchange-rates/rates?currencies=USD,EUR&targetCurrency=CZK` Query version.
- `GET  v1/api/providers` List available providers.

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
1. Implement `IExchangeRateDataProvider` to fetch and map raw data.
2. Implement `IExchangeRateProvider` (set `ExchangeRateProviderCurrencyCode`).
3. Register the implementation (`AddSingleton<IExchangeRateProvider, NewProvider>()`).
4. The factory auto-resolves by currency code.

## Configuration & Extensibility
- Logging: Console provider added by default; extend via `appsettings.json` or additional logging providers.
- Caching: FusionCache with in-memory caching and custom strategy implemented according to Providers(CNB, ECB etc) update schedule.
- Validation: Add new validators implementing `IValidator<T>` and register in DI.

## Future Enhancements
- Additional provider implementations (ECB, USD base, etc.).
- Fallback rates sources (csv,text,xml) in case of bank api downtime.
- Health checks (`/healthz`)
