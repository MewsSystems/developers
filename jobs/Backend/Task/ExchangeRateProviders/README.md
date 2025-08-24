# ExchangeRateProviders Library

A modular .NET 9 library for retrieving foreign exchange rates from pluggable data sources. It provides:

- A simple abstraction (IExchangeRateProvider) to request rates for a set of source currencies against a provider's target currency.
- A factory (IExchangeRateProviderFactory) to resolve the correct provider by its target currency code (e.g. "CZK").
- A data provider abstraction (IExchangeRateDataProvider) separating raw data acquisition from business filtering logic.
- A concrete Czech National Bank (CNB) implementation (CzkExchangeRateProvider) that exposes CZK based rates.

## Project Goals

1. Encapsulate integration details of individual central‑bank / market data feeds.
2. Provide a consistent async API with cancellation support.
3. Make adding new base‑currency providers straightforward (open/closed principle).
4. Offer good observability via structured logging.

## Key Interfaces

```csharp
public interface IExchangeRateProvider
{
    string ExchangeRateProviderCurrencyCode { get; }
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}

public interface IExchangeRateDataProvider
{
    Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken);
}

public interface IExchangeRateProviderFactory
{
    IExchangeRateProvider GetProvider(string exchangeRateProviderCurrencyCode);
}
```

## Flow

1. Client requests a provider for a base currency via ExchangeRateProviderFactory.
2. Provider fetches (or cached) full daily rate set through its IExchangeRateDataProvider.
3. Provider filters only the requested source currencies (case‑insensitive) and returns a collection of ExchangeRate.
4. Logs emit debug (fetch intent) and information (result stats) messages; validation / edge warnings if needed.

## Provided Implementation: CZK (Czech National Bank)

`CzkExchangeRateProvider`:
- Base currency: `CZK` (see `Constants.ExchangeRateProviderCurrencyCode`).
- Uses an injected `IExchangeRateDataProvider` (e.g. `CzkExchangeRateDataProviderSevice`) that calls the CNB HTTP API.
- Filters results against requested symbols for efficiency.

## Adding a New Provider

1. Implement `IExchangeRateDataProvider` for the new data source (fetch & map raw API into `IEnumerable<ExchangeRate>`).
2. Implement `IExchangeRateProvider`:
   - Expose new `ExchangeRateProviderCurrencyCode` (e.g. `"USD"`).
   - Inject its data provider + logger.
   - Reuse filtering logic (or customize if required).
3. Register in DI before the factory (factory auto‑indexes by currency code):

```csharp
services.AddSingleton<IExchangeRateProvider, UsdExchangeRateProvider>();
```

4. Consumers can now request: `factory.GetProvider("USD")`.

## Cancellation

All public async APIs accept a `CancellationToken`. Propagate tokens from HTTP endpoints, background jobs, or CLI tools to enable cooperative cancellation of downstream HTTP calls.

## Testing

- Provider behavior is unit tested (filtering, null handling, logging). 

## Notes

- Library targets .NET 9.
- Uses FusionCache for caching. Caching strategy can be customized per provider. Uses in-memory caching by default but can be extended to distributed caches.
- Designed to be extended with additional providers (e.g., ECB, Fed, custom market feeds) without modifying existing provider code.

