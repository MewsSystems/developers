# ExchangeRateProviders Library Documentation

A modular .NET 9 library for retrieving foreign exchange rates from pluggable data sources. It provides:

- A unified service interface (`IExchangeRateService`) for requesting exchange rates with target currency specification.
- A factory (`IExchangeRateDataProviderFactory`) to resolve data providers by target currency code (e.g. "CZK").
- A data provider abstraction (`IExchangeRateDataProvider`) separating raw data acquisition from business filtering logic.
- A concrete Czech National Bank (CNB) implementation (`CzkExchangeRateDataProvider`) that provides CZK-based rates.

## Project Goals

1. Encapsulate integration details of individual central‑bank / market data feeds.
2. Provide a consistent async API with cancellation support.
3. Make adding new target‑currency providers straightforward (open/closed principle).
4. Offer good observability via structured logging.
5. Separate concerns between data acquisition and business logic filtering.

## Key Interfaces
public interface IExchangeRateService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string TargetCurrency, IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}

public interface IExchangeRateDataProvider
{
    string ExchangeRateProviderTargetCurrencyCode { get; }
    Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken);
}

public interface IExchangeRateDataProviderFactory
{
    IExchangeRateDataProvider GetProvider(string exchangeRateProviderCurrencyCode);
}
## Architecture & Flow

The library follows a layered architecture with clear separation of concerns:

1. **Service Layer**: `ExchangeRateService` provides the main business logic and acts as the primary entry point.
2. **Factory Layer**: `ExchangeRateDataProviderFactory` resolves the appropriate data provider based on target currency.
3. **Provider Layer**: Individual `IExchangeRateDataProvider` implementations handle data source-specific logic.

### Request Flow

1. Client calls `IExchangeRateService.GetExchangeRatesAsync()` with target currency and requested source currencies.
2. Service uses `IExchangeRateDataProviderFactory` to resolve the appropriate data provider for the target currency.
3. Data provider fetches (or returns cached) full daily rate set from its data source.
4. Service filters results to only the requested source currencies (case‑insensitive matching).
5. Structured logs emit debug (fetch intent) and information (result stats) messages.

## Provided Implementation: CZK (Czech National Bank)

`CzkExchangeRateDataProvider`:
- Target currency: `CZK` (see `Constants.ExchangeRateProviderCurrencyCode`).
- Fetches data from Czech National Bank (CNB) HTTP API via `ICzkCnbApiClient`.
- Implements intelligent caching using FusionCache with Prague timezone-aware cache expiration.
- Maps raw CNB API responses to standardized `ExchangeRate` objects.

### Registration Example
services.AddFusionCache();
services.AddHttpClient<ICzkCnbApiClient, CzkCnbApiClient>();
services.AddSingleton<IExchangeRateDataProvider, CzkExchangeRateDataProvider>();
services.AddSingleton<IExchangeRateDataProviderFactory, ExchangeRateDataProviderFactory>();
services.AddSingleton<IExchangeRateService, ExchangeRateService>();

## Adding a New Provider

To add support for a new target currency (e.g., USD from Federal Reserve):

1. **Implement the Data Provider**:public class UsdExchangeRateDataProvider : IExchangeRateDataProvider
{
    public string ExchangeRateProviderTargetCurrencyCode => "USD";
    
    public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken)
    {
        // Fetch from Fed API, apply caching, map to ExchangeRate objects
       }
   }
2. **Register in DI Container**:services.AddSingleton<IExchangeRateDataProvider, UsdExchangeRateDataProvider>();
3. **Use the Service**:var rates = await exchangeRateService.GetExchangeRatesAsync("USD", currencies, cancellationToken);
The factory automatically discovers and indexes providers by their target currency code.

## Usage Examples

### Basic Usagevar currencies = new List<Currency> { new("USD"), new("EUR"), new("JPY") };
var rates = await exchangeRateService.GetExchangeRatesAsync("CZK", currencies, cancellationToken);

foreach (var rate in rates)
{
    Console.WriteLine($"{rate.SourceCurrency.Code} -> {rate.TargetCurrency.Code}: {rate.Value}");
}
### With Dependency Injectionpublic class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;
    
    public ExchangeRateController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRates(string targetCurrency, [FromQuery] string[] currencies)
    {
        var currencyObjects = currencies.Select(c => new Currency(c));
        var rates = await _exchangeRateService.GetExchangeRatesAsync(targetCurrency, currencyObjects, HttpContext.RequestAborted);
        return Ok(rates);
    }
}
## Caching Strategy

The library uses FusionCache for intelligent caching:

- **CZK Provider**: Uses Prague timezone-aware cache expiration (refreshes after CNB publishes new rates).
- **Extensible**: Each provider can implement its own caching strategy based on data source characteristics.
- **Configurable**: Supports both in-memory and distributed caching backends.

## Error Handling

- **Null Safety**: Handles null currency collections gracefully.
- **Provider Resolution**: Throws `InvalidOperationException` for unsupported target currencies.
- **Logging**: Comprehensive structured logging for debugging and monitoring.
- **Cancellation**: Full support for cooperative cancellation throughout the call chain.

## Testing

- Service behavior is unit tested (filtering, null handling, provider resolution, logging).
- Data provider implementations have dedicated test coverage.
- Integration tests validate end-to-end scenarios.

## Notes

- Library targets .NET 9.
- Uses FusionCache for caching with configurable strategies per provider.
- Designed for extension with additional providers (e.g., ECB, Fed, custom market feeds) without modifying existing code.
- All public async APIs accept `CancellationToken` for cooperative cancellation.
- Case-insensitive currency code matching for improved usability.

