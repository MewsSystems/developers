# Mews backend developer task

# ExchangeRateUpdater
## Overview

ExchangeRateUpdater is a .NET backend service that fetches daily foreign exchange rates from the Czech National Bank (CNB) API and exposes them via a REST API. The service is built to be production-ready, with caching, error handling, logging, and a clean architecture using MediatR, FluentValidation, and FusionCache.

The service only returns exchange rates directly provided by CNB (no calculated inverse rates), and only for the set of predefined source currencies.

## Features

- Fetches real-time daily exchange rates from CNB.

-  Caches API responses for 1 hour using FusionCache.

- Resilient HTTP requests with Polly (retry + circuit breaker).

- Implements CQRS + MediatR for query handling.

- Validation pipeline with FluentValidation.
 
- Centralized exception handling with logging of unhandled exceptions.
 
- Swagger API documentation for easy exploration.


## Endpoints


| Method | Route                            | Description                        |
|--------|----------------------------------|------------------------------------|
| GET    | `/api/exchange-rates`            | Returns the exchange rate of source currencies |

| Parameter  | Type                | Description                                                   |
| ---------- | ------------------- | ------------------------------------------------------------- |
| `date`     | string (yyyy-MM-dd) | Optional. Fetch rates for a specific date. Defaults to today. |
| `language` | string              | Optional. Language code for CNB API response. Defaults to CZ. |


## Design Decisions

- MediatR: Decouples request handling from controller logic.

- FusionCache: Caching improves performance and reduces API calls.
 
- Polly: Ensures resilience against transient network failures.
 
- Controllers: Keep thin; business logic is in the provider/service layer.
 
- ExchangeRateProvider: Filters only source currencies and does not calculate derived rates.


## Design Assumptions

- All CNB rates are relative to CZK.

- Only source currencies in Utils.SourceCurrencies are relevant.

- CNB API always provides numeric Rate and Amount.

- The service does not compute inverse rates (e.g., USD/CZK if CNB provides CZK/USD).

## Limitations

- Currently only fetches rates against CZK.

- No authentication implemented for API.

- No historical data beyond the CNB daily feed.

- Limited error handling for malformed API responses.

## Future Improvements

- Implement per-user API key or token-based authentication.

- Add historical data storage for analytics and trend tracking.

- Introduce OpenTelemetry / structured logging for observability.
 
- Add full integration tests with CNB API sandbox.
 
- Expand supported currencies dynamically from CNB API.
 
## External Libraries / Packages

- MediatR - CQRS / mediator pattern.

- FluentValidation - request validation pipeline.
 
- FusionCache - caching provider.
 
- Polly - resiliency (retry, circuit breaker).
 
- Polly.Extensions.Http - HTTP-specific policies.
 
- Microsoft.AspNetCore.Mvc - API framework.

## Next Steps

- Ensure pipeline logging captures all unhandled exceptions.

- Integrate with CI/CD pipeline to build and run tests automatically.

- Deploy to a cloud environment Azure App Service  with environment-based configuration.