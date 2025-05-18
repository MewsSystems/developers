# Exchange Rate Updater

## Overview

This project is a C# console application designed to fetch and update foreign exchange rates using data provided by the Czech National Bank (CNB). The application retrieves the latest daily exchange rates and can be integrated into financial systems or used as a standalone tool for currency conversion and rate analysis.

## Features
- Fetches up-to-date exchange rates from the official CNB source
- Parses and processes the daily exchange rate data
- Supports multiple currencies as provided by the CNB
- Modular and extensible codebase for easy integration
- Resilient to network errors and supports circuit breaker pattern
- Easily testable and extensible for new providers

## Data Source
- **Provider:** Czech National Bank (CNB)
- **URL:** [CNB Daily Exchange Rates](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt)
- The data is updated daily by the CNB and includes rates for a wide range of currencies against the Czech Koruna (CZK).

## Component and Project Structure

The solution is organized for clarity, maintainability, and extensibility:

- **Domain Layer** (`Domain/`):
  - `Currency.cs`: Value object representing a currency.
  - `ExchangeRate.cs`: Value object  representing an exchange rate between two currencies.
  - `ExchangeRateProvider.cs`: Exchange rate provider.
  - `IExchangeRateFetcher.cs`: Interface for fetching exchange rates.

- **Infrastructure Layer** (`Infrastructure/`):
  - `CNB/`:
    - `CNBExchangeRateFetcher.cs`: Fetches and parses rates from the CNB API.
    - `CNBExchangeRateParser.cs`: Parses the CNB data format.
    - `CachedExchangeRateFetcher.cs`: Adds caching to the fetcher.
    - `CNBOptions.cs`: Configuration options for the CNB provider.
  - `Observability/`:
    - `Metrics.cs`: Exposes application metrics.

- **Application Entry Point**:
  - `Program.cs`: Main entry point, orchestrates fetching and processing of exchange rates.

- **Configuration**:
  - `appsettings.json`: Application configuration (e.g., endpoints, logging).

- **Testing** (`Tests/ExchangeRateUpdaterTests/`):
  - Contains unit and integration tests for core components, using xUnit, Moq, and FluentAssertions.

This structure supports separation of concerns, testability, and future extension (e.g., adding new providers or output formats).

## Architecture & Design Decisions
- Uses dependency injection for testability and flexibility.
- Applies the circuit breaker pattern (via Polly) for resilience against network or provider failures.
- Follows SOLID principles and clean architecture for maintainability.
- Logging is integrated for observability and troubleshooting.

## Testing
- Unit tests are provided for all major components.
- Tests use xUnit, Moq for mocking dependencies, and FluentAssertions for expressive assertions.
- To run tests:
  1. Navigate to the `Tests/ExchangeRateUpdaterTests/` directory.
  2. Run `dotnet test`.

## Extensibility
- To add a new exchange rate provider, implement the `IExchangeRateFetcher` interface and register it in the DI container.
- To support new data formats, implement a new parser and inject it into the fetcher.

## Error Handling & Resilience
- Network and provider errors are handled gracefully.
- Circuit breaker pattern ensures the application remains responsive during provider outages.

## Configuration
- Endpoints, logging, and other settings are managed via `appsettings.json`.
- Update configuration as needed for different environments.

## Usage
1. Build the project using the provided solution file (`ExchangeRateUpdater.sln`).
2. Run the application. It will automatically fetch the latest exchange rates from the CNB and process them.
3. The application can be extended to store rates in a database, expose them via an API, or integrate with other systems as needed.

## Requirements
- .NET 9.0 or higher
- Internet connection to access the CNB data source

## License
This project is provided as an exercise for recruitment purposes. License terms may be specified by the author or company.
