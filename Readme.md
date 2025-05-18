# Exchange Rate Parser

A simple .NET application which fetches daily foreign exchange rates from the Czech National Bank (CNB), parses the raw data, and filters it by a specified list of currencies.

## Features

- Fetches daily foreign exchange rates from Czech National Bank (CNB).
- Parses raw text data into a structured collection of `ExchangeRate` objects.
- Calculates normalized exchange rate (price per single unit) for currencies with multiple base units (e.g., HUF, JPY, TRY, etc.).
- Filters exchange rates based on the provided list of currencies.
- Includes basic retry logic to improve resilience against temporary network or server errors.
- Handles missing and empty data as errors during parsing and filtering, ensuring only valid exchange rate information is processed.
- The project is structured using interfaces and dependency injection, making it easy to extend or replace components (e.g., for different data sources or formats).

## Design Principles

This project follows clean code and well-established software design principles including:

- **SOLID**: Ensuring code is maintainable and extensible.
- **KISS (Keep It Simple, Stupid)**: Keeping implementations straightforward and easy to understand.
- **YAGNI (You Aren’t Gonna Need It)**: The implementation avoids over-engineering by including only features required by the task. For instance, file caching was considered but intentionally left out to keep the solution focused and minimal.
- **DRY (Don't Repeat Yourself)**: Avoiding duplication to improve maintainability.

## Getting Started

### Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or newer

### Setup & Run

1. Clone the repository:
```
git clone https://github.com/dragana95/MewsExchangeRateProvider
```
2. Navigate to the project folder:
```
cd MewsExchangeRateProvider\ExchangeRateUpdater
```
3. Build the project:
```
dotnet build
```
4. Run the project:
```
dotnet run
```

### Running Unit Tests

Unit tests are written using [MSTest](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest) and [FluentAssertions](https://fluentassertions.com/).

Unit tests are in a separate project and can be run as follows:

1. Navigate to the test project folder:
```
cd MewsExchangeRateProvider\ExchangeRateUpdater.UnitTests
```
2. Run:
```
dotnet test
```

## Usage Example

Here is a quick example of how to use the `ExchangeRateService` to fetch and filter exchange rates for selected currencies:

```csharp
using System;
using System.Collections.Generic;

// Initialize dependencies
var dataFetcher = new HttpDataFetcher();
var parser = new TextParser();

// Create the service with injected dependencies
var exchangeRateService = new ExchangeRateService(dataFetcher, parser);

// Define currencies you want exchange rates for
var currencies = new List<Currency>
{
    new Currency("USD"),
    new Currency("EUR")
};

// Fetch and filter exchange rates
var exchangeRates = exchangeRateService.GetExchangeRates(currencies);

// Output results
foreach (var rate in exchangeRates)
{
    Console.WriteLine(rate);
}
```