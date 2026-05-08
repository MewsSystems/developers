# Mews backend developer task

## What This Application Does

This is a .NET 10.0 console application that fetches current exchange rates from the Czech National Bank (CNB) and displays them in CZK-to-foreign-currency format.

The application:
- Fetches daily exchange rate data from CNB's public API (pipe-separated text format)
- Parses the data and converts rates to CZK/XXX format (1 CZK = X foreign currency units)
- Returns only explicitly defined rates (no calculated or inverse rates)
- Silently ignores currencies not available from CNB

## Design Decisions

**Data Source**: CNB daily exchange rates API (`https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt`)
- Official source, pipe-separated format, updated daily
- Provides rates as: Amount units of foreign currency = Rate CZK

**Architecture**:
- Dependency injection for loose coupling and testability
- Separation of concerns: HTTP client, parser, and provider
- Polly retry policy for transient HTTP failures (3 retries with 2-second delays)
- Configuration-based settings for timeouts and retry behavior

**Error Handling**:
- HTTP errors are logged and propagated with clear messages
- Missing currencies are silently ignored per requirements
- Timeout errors are explicitly handled

## Possible Improvements

**Caching**: Add in-memory caching with time-based expiration (CNB updates daily at 2:15 PM CET)

**Rate Conversion**: Support inverse rate calculation (e.g., USD/CZK from CZK/USD) if business requirements change

**Data Source Fallback**: Add secondary data source in case CNB API is unavailable

**Observability**: Add structured logging with correlation IDs for better troubleshooting

## Running the .NET Exchange Rate Updater

### Prerequisites
- .NET 10.0 SDK

### Build and Run the Application

```bash
# Build the solution
dotnet build Task/ExchangeRateUpdater.sln

# Run the application
dotnet run --project Task/ExchangeRateUpdater.csproj
```

The application will fetch current exchange rates from the Czech National Bank and display them in the console.

### Run Unit Tests

```bash
# Run all tests
dotnet test Task.Tests/ExchangeRateUpdater.Tests.csproj

# Run tests with detailed output
dotnet test Task.Tests/ExchangeRateUpdater.Tests.csproj --verbosity normal
```