# Architecture & Decisions
Context and goals
This document describes the architecture and key decisions made while implementing the ExchangeRateProvider for the Czech National Bank (CNB). The goal is to build a small but production-ready component that focuses on clarity, testability, and resilience rather than on feature completeness.

## Target framework choice (.NET 10)

The original skeleton project was provided targeting .NET 6. For the implementation, I updated the target framework to .NET 10 (`net10.0`) for the following reasons:

- .NET 10 is a Long Term Support (LTS) release, supported for three years, which makes it a stable choice for production-oriented components.
- It provides the latest runtime and library improvements, including performance and reliability enhancements that benefit long-running services and network-bound components such as an exchange rate provider.


## Data source (Czech National Bank)
The provider uses an official public data source published by the Czech National Bank. The chosen endpoint exposes daily exchange rates for multiple currencies against CZK and is suitable for server-side consumption. The implementation assumes the format to be stable and documented, and it includes basic validation to detect unexpected changes in the response structure.

## Domain model and contracts
The implementation follows the interface and expectations defined in ExchangeRateProvider.cs. The primary responsibility of the provider is to deliver exchange rates for a given date and currency in a strongly-typed way. Any parsing or transport-specific details are kept internal so that consumers only see a simple, domain-focused API.

## Solution design
The solution is structured around a clear separation of concerns. An HTTP client component is responsible for calling the CNB endpoint and retrieving the raw response. A parser component translates the CNB response format into domain objects that represent exchange rates. The ExchangeRateProvider orchestrates these pieces and exposes a simple method to obtain exchange rates for the rest of the system. This separation makes the code easier to test and maintain.

## Error handling and resilience
The provider handles common failure scenarios explicitly, such as network errors, timeouts, invalid responses, or missing exchange rates. In these cases, it either throws meaningful exceptions or returns clearly defined results, depending on the contract. Input parameters (such as dates or currency codes) are validated early to avoid ambiguous behavior. Timeouts and basic guards are used to prevent the application from hanging when the external service is slow or unavailable.
​

## Testing and validation
The implementation is covered by unit tests focusing on the most critical logic: parsing of the CNB response, correct mapping of raw data to domain models, and behavior in error scenarios (for example, unsupported currencies or malformed responses). External dependencies such as HTTP calls are abstracted behind interfaces so tests can run deterministically without hitting the real CNB endpoint.

## Maintainability and extensibility
The design aims to make future changes as localized as possible. Adding another bank or data source would typically require introducing a new provider implementation and possibly a new parser, without affecting existing consumers. Configuration points (such as base URLs or timeouts) are kept outside of the core logic so they can be adjusted without code changes.

## Future improvements
Potential future improvements include caching daily exchange rates to reduce the number of external calls, exposing configuration for retry policies and resilience strategies, and adding support for additional CNB endpoints (for example, historical data or different formats). Another possible enhancement is richer logging around failures to simplify diagnostics in production environments.