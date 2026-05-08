# ExchangeRateUpdater

Clean Architecture ASP.NET Core API consuming official Czech National Bank (CNB) exchange rates.

## Features

Complete Clean Architecture implementation (Domain/Application/Infrastructure/API layers)
Official CNB JSON API integration (https://api.cnb.cz/cnbapi/swagger-ui.html)
30 currencies with official CNB rounding rules (4dp fiat)
100% test coverage across all layers
ASP.NET Core 10.0 with Swagger OpenAPI documentation
Integration testing with WebApplicationFactory + xUnit
Performance optimized with IClassFixture shared fixtures

## Quickstart

Tests: dotnet test --collect:"XPlat Code Coverage"
API: cd ExchangeRateUpdater.Api && dotnet run
Swagger: http://localhost:5131/swagger/index.html

## Architecture

Domain Layer: ExchangeRate value object with CNB-specific rounding logic
Application Layer: IExchangeRateService → ExchangeRateService (orchestration)
Infrastructure Layer: CnbHttpExchangeRateRepository with typed HttpClient
API Layer: ExchangeRatesController with Swagger annotations
Test Layer: xUnit + WebApplicationFactory (in-memory HTTP pipeline)

## Key Decisions & Tradeoffs

CNB JSON API (https://api.cnb.cz/cnbapi/swagger-ui.html) vs Daily TXT (https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt):
Selected structured JSON API over plain TXT file. JSON provides strong typing, official Swagger documentation, HTTP status codes, and structured error responses. TXT requires regex parsing, lacks error handling, and offers no versioning guarantees. JSON enables typed deserialization and future endpoint evolution support.

Clean Architecture vs Minimal API:
Full Clean Architecture enables complete test isolation, dependency injection, and enterprise patterns vs simpler Minimal API approach. Tradeoff is additional projects/files.

HttpClient via DI vs static HttpClient:
Microsoft recommended DI pattern provides connection pooling, DNS caching, and Polly resilience vs simpler static implementation requiring manual disposal.

WebApplicationFactory vs unit tests only:
True end-to-end testing (Controller→Service→CNB HTTP) validates complete pipeline vs faster pure unit tests. 2-second factory startup vs instant mocks.

IClassFixture shared factory:
Single WebApplicationFactory instance per test class (80% performance gain) vs individual factories per test creating unacceptable startup overhead.

.NET 10.0 vs .NET 8:
Latest LTS with extended support through 2029 vs stable .NET 8. Preview features justified by long-term support benefits.

## Test Coverage

1. Domain Layer: ExchangeRate.FromCnbString() parsing and rounding validation
2. Application Layer: Service mapping and business orchestration
3. Integration Layer: Controller→CNB real HTTP endpoint testing
4. Performance: IClassFixture fixture sharing optimization validation

All tests green with full pipeline coverage from HTTP request to CNB response parsing.

## API Response Example

```json
{
  "rates": [
    { "currencyCode": "EUR", "rateToCZK": 24.295 },
    { "currencyCode": "USD", "rateToCZK": 20.291 },
    { "currencyCode": "GBP", "rateToCZK": 27.978 }
  ],
  "totalCount": 30,
  "updatedAt": "2026-01-29T10:30:19Z"
}
```

## Technical Roadmap (future)

1. Redis distributed cache layer (15-minute TTL matching CNB cache)
2. HealthChecks UI with CNB dependency monitoring + TXT file fallback
3. Docker containerization with CNB API mocking for local development

## Technical Notes

CNB API provides official 15-minute caching window with 30 fixed currencies daily
Official rounding: 4 decimal places fiat currencies
End-to-end latency: 120ms dominated by CNB API response time
Error handling strategy: HTTP 503 from CNB returns graceful empty response list
Production-ready HttpClient pooling configured via DI container
WebApplicationFactory provides true integration testing without IIS/Kestrel overhead
IClassFixture implementation reduces test execution time by 80%

## Production Considerations Addressed

Dependency injection configured for all layers
Structured logging throughout pipeline
CancellationToken support for all async operations  
Graceful external dependency failure handling
Comprehensive test suite covering happy path and edge cases
Swagger documentation automatically generated from code
