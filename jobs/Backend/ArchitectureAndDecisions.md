# Architecture & Decisions

## Context and goals

This document describes the architecture and key decisions made while implementing an `ExchangeRateProvider` for the Czech National Bank (CNB). The goal is to build a small but production-ready set of components that focus on clarity, testability, and resilience rather than on feature completeness. The solution is structured so that the core abstractions and CNB-specific implementation can be reused from different hosts.

## Target framework choice (.NET 10)

The original skeleton project was provided targeting .NET 6. For the implementation, I updated the target framework to .NET 10 (`net10.0`) for the following reasons:

- .NET 10 is a Long Term Support (LTS) release, supported for three years, which makes it a stable choice for production-oriented components.
- It provides the latest runtime and library improvements, including performance and reliability enhancements.


## Project structure

The solution is split into several projects to keep responsibilities clear and to mirror a realistic architecture:

- `ExchangeRateUpdater.Abstraction`  
  Contains the core domain model and contracts:
  - `Model/` with `Currency` and `ExchangeRate` as strongly-typed value objects.
  - `Interfaces/` with `IExchangeRateClient`, `IExchangeRateProvider`, and `IExchangeRateService`.  
  This project has no external dependencies other than the base class library.

- `ExchangeRateUpdater.CbnApiClient`  
  Encapsulates the integration with the Czech National Bank API:
  - `Dto/` contains transport-level models (`ExchangeRatesResponse`, `ExchangeRateEntry`) that match the CNB API payload.
  - `Implementation/ExchangeRateCnbApiClient` implements `IExchangeRateClient` using `HttpClient`.
  - `Mapper/CnbExchangeRateMapperExtensions` converts CNB DTOs into domain `ExchangeRate` instances.  
  The `CnbApiInfo.md` file documents the chosen endpoint, format, and behaviour of `validFor`.

- `ExchangeRateUpdater.Service`  
  Contains the application-level services:
  - `ExchangeRateProvider` implements `IExchangeRateProvider` on top of `IExchangeRateClient`. It applies domain rules, filtering, and caching on top of the raw CNB data.
  - `ExchangeRateService` implements `IExchangeRateService` and acts as an application service that maps input (currency codes as strings) to domain types and orchestrates calls to the provider.

- `ExchangeRateUpdater.Api`  
  Hosts an ASP.NET Core Web API:
  - `Controllers/ExchangeRateController` exposes a HTTP endpoint to retrieve exchange rates for a set of currency codes.
  - `Middleware/ExceptionHandlingMiddleware` centralizes exception handling and error responses.
  - `Program.cs` wires the DI container, logging, and middleware pipeline.  
  The API is intentionally thin: it focuses on HTTP concerns and delegates business logic to `IExchangeRateService`.

- `ExchangeRateUpdater.DependencyInjection`  
  Provides a reusable DI setup:
  - `ExchangeRateCnbServicesExtension` exposes an `AddExchangeRateServices(IServiceCollection)` extension method that registers `IExchangeRateProvider`, `IExchangeRateService`, and the CNB HTTP client with resilience policies.
  - `DependencyInyection.md` documents why this configuration is kept in a reusable project.  
  This allows both the API and a console application to share the same wiring without duplicating configuration.

- `ExchangeRateUpdater.Tests`  
  Contains automated tests organized by area:
  - `Abstraction/`, `Api/`, `CnbApiClient/`, `Service/` and `IntegrationTests/`.  
  Unit tests target domain classes, mapping logic, and provider behaviour; integration tests cover the end-to-end path through the service and API layers.

## Data source (Czech National Bank)

The provider uses an official public data source published by the Czech National Bank. The chosen endpoint returns the full daily set of foreign exchange rates, each entry including a `validFor` date that indicates when the rate applies.

Relevant characteristics:

- Rates are published on business days (weekdays) and updated once per day, around 14:30 UTC, according to CNB documentation.
- The endpoint always returns the full set of available currency entries; there is no server-side filtering by currency.
- When queried on weekends or holidays, the API returns the most recently published rates, i.e., the latest `validFor` date prior to the request.

Because of these characteristics, the client always requests the full daily rates payload and leaves filtering and caching to upper layers.

## Domain model and contracts

The domain model is intentionally small and focused:

- `Currency` is a value object that represents a three-letter ISO 4217 currency code. It enforces its invariants in the constructor: the code must be non-empty, exactly three characters long, and composed of letters only. The code is normalized using `ToUpperInvariant()`, and equality/hash code use a case-insensitive comparison to avoid culture-specific issues.
- `ExchangeRate` represents a rate between a base currency (CZK in this case), a quoted currency, and a validity date.

Contracts are defined in the `Interfaces` namespace:

- `IExchangeRateClient` abstracts the CNB integration. It is responsible for retrieving raw exchange rate data from the external API, but it does not apply business rules or filtering.
- `IExchangeRateProvider` represents a domain-facing abstraction that returns exchange rates for a given date and set of currencies.
- `IExchangeRateService` is an application-level service used by the API. It accepts currency codes as strings, maps them to `Currency` objects, and delegates to `IExchangeRateProvider`.

This separation allows the Web API to depend only on abstractions, while the CNB-specific details are encapsulated in infrastructure projects.

## Solution design

The solution follows a layered design aligned with clean architecture principles:

- **Domain (Abstraction project)**  
  Contains the core types and contracts with no external dependencies.

- **Infrastructure (CbnApiClient project)**  
  Integrates with CNB using `HttpClient`. The client:
  - Calls the official endpoint to fetch the full daily exchange rates payload.
  - Deserializes the response into `ExchangeRatesResponse` and `ExchangeRateEntry`.
  - Maps DTOs into domain-level `ExchangeRate` instances via `CnbExchangeRateMapperExtensions`.

- **Application services (Service project)**  
  - `ExchangeRateProvider` uses `IExchangeRateClient` to retrieve the latest or requested daily rates. It then:
    - Filters the full set to the requested currencies.
    - Applies basic caching to avoid repeated calls for the same date.
  - `ExchangeRateService` provides a higher-level API for hosts: it converts comma-separated currency codes into `Currency` objects and delegates the actual rate retrieval to the provider.

- **Presentation (Api project)**  
  - `ExchangeRateController` is a thin Web API controller that:
    - Accepts a `currencyCodes` query parameter.
    - Splits and trims the codes.
    - Calls `IExchangeRateService` and returns the resulting exchange rates.
  - `ExceptionHandlingMiddleware` converts unhandled exceptions into consistent HTTP responses and logs them.

- **Composition (DependencyInjection project)**  
  - `ExchangeRateCnbServicesExtension` centralizes DI registration so that multiple hosts can share the same wiring and resilience configuration.

This separation of concerns keeps each project focused on a single responsibility, makes the code easier to test, and makes it clear where to make changes when integrating a different data source.

## Error handling and resilience

Error handling is applied at several levels:

- **Domain validation**  
  `Currency` validates its input in the constructor and throws `ArgumentException` for invalid codes (null/whitespace, wrong length, or invalid characters). This ensures that any `Currency` instance used by the provider is always in a valid state.

- **HTTP resilience (CNB client)**  
  The CNB HTTP client is registered via `AddHttpClient<IExchangeRateClient, ExchangeRateCnbApiClient>()` and configured with a Polly retry policy:
  - Transient HTTP errors (5xx, timeouts, etc.) are retried up to three times.
  - The wait times use exponential backoff (2, 4, and 8 seconds).
  This provides basic resilience against temporary CNB outages or network glitches without adding complex logic to the client itself.

- **API-level exception handling**  
  In the Web API project, `ExceptionHandlingMiddleware` catches unhandled exceptions, logs them, and returns standardized error responses. This avoids leaking implementation details to clients.

- **Input validation at the API boundary**  
  The API controller validates and normalizes the `currencyCodes` query parameter (split, trim, ignore empty entries) before delegating to the service.

## Caching strategy

Given CNB's update behaviour, caching is particularly effective for this use case. Daily FX rates are published once on business days around 14:30 UTC and remain valid for the entire day (and weekends/holidays until the next business day). The API always returns the latest available set of rates for the most recent `validFor` date.

The `ExchangeRateProvider` uses an in-memory cache based on `IMemoryCache`. The cache stores the full set of CNB rates under a single key and applies time-based expiration:

- When exchange rates are requested, the provider first checks the cache.
- On a cache miss, it calls `IExchangeRateClient` to fetch the latest daily rates from CNB and stores the result in the cache with an absolute expiration.
- On a cache hit, the provider simply filters the cached rates to the requested currencies, avoiding another HTTP call.

The expiration is aligned with CNB’s daily publication schedule. The provider computes the next expected publication time (15:00 UTC with a small safety margin) and sets the cache entry to expire at that moment. This means:

- For most of the day, repeated requests are served entirely from memory.
- Shortly after CNB publishes new rates, the next request will trigger a refresh and cache the new daily set.
- A minimum and maximum duration (between 1 minute and 24 hours) guard against edge cases in date/time calculations.

This approach keeps the implementation simple while significantly reducing the number of external calls, which is a common recommendation for currency/exchange-rate APIs.


## Testing and validation

Automated tests are an essential part of this solution. The goal is not only to demonstrate correctness for a few happy paths, but also to make the behaviour of the provider, services, and API predictable when the implementation evolves.

The test suite covers domain types, CNB client mapping, caching behaviour, and the main API flow. External dependencies such as HTTP calls are abstracted behind interfaces (`IExchangeRateClient`), so tests run deterministically without hitting the real CNB endpoint. This makes it easy to validate edge cases (for example, unsupported currencies or malformed responses) in isolation.

Code coverage is used as an additional feedback metric. A coverage report (`coveragereport/`) and a screenshot (`CodeCoverage.png`) are included at the root of the solution to make evaluation of the assignment easier. In a real production repository, these generated artefacts would typically not be committed, but produced by the build/CI pipeline instead. 

Most classes reach close to 100% coverage, and all core components have at least 85% coverage, ensuring that the critical paths of the system are exercised by tests. The only notable exception is `ExceptionHandlingMiddleware`, which currently has lower coverage as it mainly deals with cross-cutting concerns and framework integration, and is therefore less critical for the core domain logic.


## Maintainability and extensibility

The design aims to make future changes as localized as possible:

- Adding support for another bank or data source would typically involve:
  - Introducing a new `IExchangeRateClient` implementation and mapper in a separate infrastructure project.
  - Optionally adding another `IExchangeRateProvider` implementation if the semantics differ.
- The Web API depends only on abstractions (`IExchangeRateService`, `IExchangeRateProvider`, `IExchangeRateClient`), which makes it easier to extend or replace infrastructure without touching controllers.
- Configuration points such as base URLs, timeouts, and retry policies live in the DI setup and application configuration (`appsettings.json`), not in the core logic, so they can be adjusted without code changes.

## Future improvements

Potential future improvements include:

- Introducing a more advanced caching strategy (e.g., configurable expiration or a distributed cache) if the service runs in a multi-instance environment.
- Exposing configuration for retry policies and resilience strategies (number of retries, backoff function) via configuration rather than hard-coding them.
- Adding richer logging and metrics around CNB requests (latency, failure rates, cache hit rates) to simplify diagnostics and capacity planning.
- Extending the API to support historical queries and additional CNB endpoints (for example, endpoints that allow querying by arbitrary date) as described in the CNB documentation.
