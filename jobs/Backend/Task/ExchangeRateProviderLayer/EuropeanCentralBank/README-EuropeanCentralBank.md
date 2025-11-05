# European Central Bank (ECB) Exchange Rate Provider

Implementation of exchange rate provider for the **European Central Bank**.

## Overview

This provider fetches daily exchange rates from the European Central Bank's public XML API. The ECB publishes reference exchange rates for major currencies against the Euro (EUR).

**Configuration is loaded from ConfigurationLayer** via `IProviderConfigurationService` (database-first with appsettings.json fallback).

### Provider Information

- **Name**: European Central Bank
- **Code**: ECB
- **Base Currency**: EUR (Euro) - configurable
- **API Endpoint**: https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml - configurable
- **Data Format**: XML (triple-nested Cube structure)
- **Update Frequency**: Daily (typically updated around 16:00 CET)
- **Authentication**: Not required (public API)
- **Rate Limit**: None specified
- **Historical Data**: Available (90 days and full history endpoints)

## Features

✅ Fetches latest daily exchange rates
✅ XML parsing with automatic deserialization
✅ Triple-nested Cube structure handling (Envelope > Cube > Cube > Cube)
✅ Comprehensive error handling
✅ Type-safe data models
✅ Standardized DTO conversion
✅ Timeout protection (30 seconds)
✅ Database-first configuration with appsettings.json fallback
✅ Historical data endpoints available

## Implementation

### Architecture

```
EuropeanCentralBank/
├── Models/
│   └── EcbEnvelope.cs          # XML response models
├── Converters/
│   └── EcbConverter.cs         # Converts ECB response to ExchangeRateDTO
├── EuropeanCentralBankProvider.cs  # Main provider implementation
└── README-EuropeanCentralBank.md
```

### Components

#### 1. **EcbEnvelope** (Models)

XML response models matching the ECB API structure:

```csharp
[XmlRoot("Envelope", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
public class EcbEnvelope
{
    [XmlElement("subject", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
    public string? Subject { get; set; }

    [XmlElement("Sender", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
    public EcbSender? Sender { get; set; }

    [XmlElement("Cube", Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
    public EcbOuterCube? Cube { get; set; }
}

// Triple-nested Cube structure
public class EcbOuterCube
{
    [XmlElement("Cube", Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
    public List<EcbDateCube> DateCubes { get; set; } = new();
}

public class EcbDateCube
{
    [XmlAttribute("time")]
    public string? Time { get; set; }

    [XmlElement("Cube", Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
    public List<EcbRate> Rates { get; set; } = new();
}

public class EcbRate
{
    [XmlAttribute("currency")]
    public string? Currency { get; set; }

    [XmlAttribute("rate")]
    public decimal Rate { get; set; }
}
```

#### 2. **EcbConverter**

Implements `IExchangeRateConverter<EcbEnvelope>` to transform ECB's XML response into standardized `ExchangeRateDTO` objects.

**Conversion Logic:**
- Base currency: Always EUR
- Target currency: From `currency` attribute
- Rate: Direct value from `rate` attribute
- Multiplier: Always 1 (ECB doesn't use variable amounts)

**Validation:**
- Skips rates with missing/invalid currency codes
- Skips rates with zero or negative values
- Throws if no valid rates found

#### 3. **EuropeanCentralBankProvider**

Main provider implementing `IExchangeRateProvider`.

**Key Features:**
- Async HTTP requests with 30-second timeout
- XML deserialization using `XmlSerializer`
- Comprehensive exception handling
- Standardized status codes and messages
- Proper HttpClient disposal management

## Usage

### Configuration

The provider uses **database-first configuration** with appsettings.json as fallback:

1. **Database** (Primary source) - Configuration stored in `ExchangeRateProvider` and `ExchangeRateProviderConfiguration` tables
2. **appsettings.json** (Fallback) - Used when database is unavailable or provider not found
3. **Cache** (15-minute TTL) - Improves performance by caching configurations

#### Database Configuration

Provider configuration is stored in two tables:

**ExchangeRateProvider table:**
```sql
INSERT INTO ExchangeRateProvider (Name, Code, Url, BaseCurrencyId, IsActive, RequiresAuthentication)
VALUES ('European Central Bank', 'ECB', 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml', @EurId, 1, 0);
```

**ExchangeRateProviderConfiguration table:**
```sql
INSERT INTO ExchangeRateProviderConfiguration (ProviderId, SettingKey, SettingValue, Description)
VALUES
    (@EcbId, 'Format', 'XML', 'Response format (XML with gesmes namespace)'),
    (@EcbId, 'XmlNamespace', 'http://www.gesmes.org/xml/2002-08-01', 'gesmes XML namespace'),
    (@EcbId, 'XmlStructure', 'TripleNestedCube', 'Uses Envelope > Cube > Cube > Cube structure'),
    (@EcbId, 'UpdateTime', '16:00', 'Daily update time (around 16:00 CET)'),
    (@EcbId, 'TimeZone', 'CET', 'Central European Time'),
    (@EcbId, 'HasVariableAmounts', 'false', 'Always uses 1 unit'),
    (@EcbId, 'Historical90DaysUrl', 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml', 'Last 90 days historical rates'),
    (@EcbId, 'HistoricalAllUrl', 'https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml', 'All historical rates (large file)');
```

See `Database/Script/SeedExchangeRateProvidersAndConfiguration.sql` for complete seed data.

#### Fallback Configuration (appsettings.json)

```json
{
  "ExchangeRateProviders": {
    "ECB": {
      "Name": "European Central Bank",
      "Code": "ECB",
      "Url": "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
      "BaseCurrency": "EUR",
      "IsActive": true,
      "RequiresAuthentication": false,
      "Configuration": {
        "Format": "XML",
        "DecimalSeparator": "Dot",
        "UpdateTime": "16:00",
        "TimeZone": "CET",
        "XmlNamespace": "http://www.gesmes.org/xml/2002-08-01",
        "HasVariableAmounts": false,
        "XmlStructure": "TripleNestedCube",
        "Historical90DaysUrl": "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml",
        "HistoricalAllUrl": "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml"
      }
    }
  }
}
```

### Basic Usage (Recommended)

**Using IProviderConfigurationService (Database-first):**

```csharp
using EuropeanCentralBank;
using ConfigurationLayer.Interface;

// Inject IProviderConfigurationService
private readonly IProviderConfigurationService _providerConfig;
private readonly IHttpClientFactory _httpClientFactory;

public async Task FetchEcbRates()
{
    // Load configuration from database (or fallback to appsettings.json)
    var ecbOptions = await _providerConfig.GetProviderConfigurationAsync("ECB");

    if (ecbOptions == null)
    {
        Console.WriteLine("ECB provider not configured");
        return;
    }

    // Create provider with configuration
    var httpClient = _httpClientFactory.CreateClient();
    var provider = new EuropeanCentralBankProvider(ecbOptions, httpClient);

    // Fetch today's rates
    var (status, rates) = await provider.GetExchangeRatesForToday();

    // Check status
    if (status.Item1 == 200) // HTTP 200 OK
    {
        Console.WriteLine($"Success: {status.Item2}");

        foreach (var rate in rates)
        {
            Console.WriteLine($"1 {rate.BaseCurrencyCode} = {rate.Rate} {rate.TargetCurrencyCode}");
        }
    }
    else
    {
        Console.WriteLine($"Error {status.Item1}: {status.Item2}");
    }
}
```

### With Dependency Injection (Database-first)

```csharp
// Program.cs / Startup.cs
var builder = WebApplication.CreateBuilder(args);

// Register DataLayer (DbContext, repositories)
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ConfigurationLayer services
builder.Services.AddMemoryCache();
builder.Services.Configure<ExchangeRateProvidersOptions>(
    builder.Configuration.GetSection("ExchangeRateProviders")); // Fallback config
builder.Services.AddSingleton<IProviderConfigurationService, ProviderConfigurationService>();

// Register HttpClient
builder.Services.AddHttpClient();

// In your service
public class ExchangeRateService
{
    private readonly IProviderConfigurationService _providerConfig;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(
        IProviderConfigurationService providerConfig,
        IHttpClientFactory httpClientFactory,
        ILogger<ExchangeRateService> logger)
    {
        _providerConfig = providerConfig;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task FetchEcbRates()
    {
        // Load configuration from database (with appsettings.json fallback)
        var ecbOptions = await _providerConfig.GetProviderConfigurationAsync("ECB");

        if (ecbOptions == null || !ecbOptions.IsActive)
        {
            _logger.LogWarning("ECB provider is not configured or not active");
            return;
        }

        // Create provider with loaded configuration
        var httpClient = _httpClientFactory.CreateClient();
        var provider = new EuropeanCentralBankProvider(ecbOptions, httpClient);

        // Fetch rates
        var (status, rates) = await provider.GetExchangeRatesForToday();

        if (status.Item1 == 200)
        {
            _logger.LogInformation("Successfully fetched {Count} exchange rates from ECB", rates.Count);
            // Process rates...
        }
        else
        {
            _logger.LogError("Failed to fetch ECB rates: {StatusCode} - {Message}",
                status.Item1, status.Item2);
        }
    }

    public async Task FetchAllActiveProviders()
    {
        // Load all active provider configurations from database
        var providers = await _providerConfig.GetAllActiveProviderConfigurationsAsync();

        foreach (var providerOptions in providers)
        {
            _logger.LogInformation("Processing provider: {ProviderName}", providerOptions.Name);

            // Create appropriate provider based on code
            if (providerOptions.Code == "ECB")
            {
                var httpClient = _httpClientFactory.CreateClient();
                var provider = new EuropeanCentralBankProvider(providerOptions, httpClient);
                var (status, rates) = await provider.GetExchangeRatesForToday();
                // Process rates...
            }
            // Handle other providers (BNR, CNB, etc.)
        }
    }
}
```

### Configuration Priority Flow

```
┌─────────────────────────────────────────┐
│ IProviderConfigurationService Request   │
│ GetProviderConfigurationAsync("ECB")    │
└──────────────┬──────────────────────────┘
               ↓
       ┌───────────────┐
       │  Check Cache  │
       │  (15 min TTL) │
       └───┬───────────┘
           │
    Found? │ Yes → Return cached config
           ↓ No
       ┌───────────────────────┐
       │  Query Database       │
       │  ExchangeRateProvider │
       └───┬───────────────────┘
           │
    Found? │ Yes → Cache & Return
           ↓ No
       ┌───────────────────────┐
       │  Load from            │
       │  appsettings.json     │
       └───┬───────────────────┘
           │
    Found? │ Yes → Cache & Return
           ↓ No
       Return null
```

## API Response Example

### XML Response from ECB

```xml
<?xml version="1.0" encoding="UTF-8"?>
<gesmes:Envelope xmlns:gesmes="http://www.gesmes.org/xml/2002-08-01" xmlns="http://www.ecb.int/vocabulary/2002-08-01/eurofxref">
  <gesmes:subject>Reference rates</gesmes:subject>
  <gesmes:Sender>
    <gesmes:name>European Central Bank</gesmes:name>
  </gesmes:Sender>
  <Cube>
    <Cube time='2025-11-05'>
      <Cube currency='USD' rate='1.1492'/>
      <Cube currency='JPY' rate='123.45'/>
      <Cube currency='GBP' rate='0.8574'/>
      <Cube currency='CHF' rate='1.0234'/>
      <Cube currency='CAD' rate='1.5123'/>
      <!-- ... more rates ... -->
    </Cube>
  </Cube>
</gesmes:Envelope>
```

### Converted Output

```csharp
[
  {
    BaseCurrencyCode = "EUR",
    TargetCurrencyCode = "USD",
    Multiplier = 1,
    Rate = 1.1492
  },
  {
    BaseCurrencyCode = "EUR",
    TargetCurrencyCode = "JPY",
    Multiplier = 1,
    Rate = 123.45
  },
  // ... more rates
]
```

## Rate Interpretation

ECB rates represent: **1 EUR = [Rate] [Foreign Currency]**

### Examples:

**XML Rate:**
```xml
<Cube currency="USD" rate="1.1492"/>
```

**Interpretation:**
- 1 EUR = 1.1492 USD
- To convert EUR to USD: multiply by rate
- To convert USD to EUR: divide by rate

**Another Example:**
```xml
<Cube currency="GBP" rate="0.8574"/>
```

**Interpretation:**
- 1 EUR = 0.8574 GBP
- To convert EUR to GBP: multiply by 0.8574
- To convert GBP to EUR: divide by 0.8574

## Status Codes

The provider returns standardized HTTP-like status codes:

| Code | Meaning | Description |
|------|---------|-------------|
| 200 | Success | Rates fetched successfully |
| 408 | Timeout | Request timed out (> 30 seconds) |
| 500 | Server Error | XML parsing or validation error |
| 503 | Service Unavailable | Network error, ECB API unreachable |
| Other | HTTP Status | Actual HTTP status from ECB API |

### Example Status Messages

```csharp
// Success
(200, "European Central Bank: Successfully retrieved 32 exchange rates")

// Network error
(503, "European Central Bank: Network error - No such host is known")

// Timeout
(408, "European Central Bank: Request timeout - The operation was canceled")

// Validation error
(500, "European Central Bank: Data validation error - No valid exchange rates found")
```

## Error Handling

The provider catches and handles various error scenarios:

### Network Errors
```csharp
catch (HttpRequestException ex)
{
    return ((503, $"{ProviderName}: Network error - {ex.Message}"),
            new List<ExchangeRateDTO>());
}
```

### Timeouts
```csharp
catch (TaskCanceledException ex)
{
    return ((408, $"{ProviderName}: Request timeout - {ex.Message}"),
            new List<ExchangeRateDTO>());
}
```

### Data Validation
```csharp
catch (InvalidOperationException ex)
{
    return ((500, $"{ProviderName}: Data validation error - {ex.Message}"),
            new List<ExchangeRateDTO>());
}
```

## Historical Data

The ECB provides multiple historical data endpoints:

### Available Endpoints

1. **Daily Rates** (current)
   - URL: `https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml`
   - Contains: Latest daily rates

2. **Last 90 Days**
   - URL: `https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml`
   - Contains: Historical rates for the last 90 days
   - File size: ~100 KB

3. **All Historical Data**
   - URL: `https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml`
   - Contains: All rates since 1999
   - File size: ~10 MB (large!)

### Current Implementation
```csharp
var (status, rates) = await provider.GetHistoryExchangeRates();
// Returns: Latest available rates with historical URL information
```

### Future Enhancement Options

**Example Implementation for Historical Data:**
```csharp
public async Task<List<ExchangeRateDTO>> GetHistoricalRates(DateTime startDate, DateTime endDate)
{
    // Determine which endpoint to use based on date range
    string url;
    var daysDiff = (DateTime.Now - startDate).TotalDays;

    if (daysDiff <= 90)
    {
        // Use 90-day endpoint for recent history
        url = _options.Configuration["Historical90DaysUrl"];
    }
    else
    {
        // Use full history endpoint (large file!)
        url = _options.Configuration["HistoricalAllUrl"];
    }

    // Fetch and filter by date range
    // ...
}
```

## Supported Currencies

The ECB typically publishes rates for 30+ currencies including:

**Major Currencies:**
- USD (US Dollar)
- JPY (Japanese Yen)
- GBP (British Pound)
- CHF (Swiss Franc)
- CNY (Chinese Yuan)

**European:**
- DKK (Danish Krone)
- SEK (Swedish Krona)
- NOK (Norwegian Krone)
- PLN (Polish Zloty)
- CZK (Czech Koruna)
- HUF (Hungarian Forint)
- RON (Romanian Leu)
- BGN (Bulgarian Lev)

**Others:**
- AUD, BRL, CAD, HKD, IDR, ILS, INR, ISK, KRW, MXN, MYR, NZD, PHP, SGD, THB, TRY, ZAR, and more

**Note:** Currency availability may change based on ECB policy.

## Performance Considerations

### Caching Recommendations

Since ECB rates update once daily (around 16:00 CET), caching is highly recommended:

```csharp
public class CachedEcbProvider
{
    private readonly EuropeanCentralBankProvider _provider;
    private readonly IMemoryCache _cache;

    public async Task<List<ExchangeRateDTO>> GetCachedRates()
    {
        var cacheKey = $"ECB_Rates_{DateTime.UtcNow:yyyy-MM-dd}";

        if (!_cache.TryGetValue(cacheKey, out List<ExchangeRateDTO> rates))
        {
            var (status, fetchedRates) = await _provider.GetExchangeRatesForToday();

            if (status.Item1 == 200)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));

                _cache.Set(cacheKey, fetchedRates, cacheOptions);
                return fetchedRates;
            }
        }

        return rates;
    }
}
```

### Retry Logic

Implement retry for transient failures:

```csharp
using Polly;

var retryPolicy = Policy
    .HandleResult<((int, string), List<ExchangeRateDTO>)>(
        result => result.Item1.Item1 >= 500)
    .WaitAndRetryAsync(3,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var result = await retryPolicy.ExecuteAsync(
    () => provider.GetExchangeRatesForToday());
```

## Testing

### Unit Test Example

```csharp
[Fact]
public async Task GetExchangeRatesForToday_ReturnsValidRates()
{
    // Arrange
    var options = new ExchangeRateProviderOptions
    {
        Name = "European Central Bank",
        Code = "ECB",
        Url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
        BaseCurrency = "EUR",
        IsActive = true
    };
    var provider = new EuropeanCentralBankProvider(options);

    // Act
    var (status, rates) = await provider.GetExchangeRatesForToday();

    // Assert
    Assert.Equal(200, status.Item1);
    Assert.NotEmpty(rates);
    Assert.All(rates, rate =>
    {
        Assert.Equal("EUR", rate.BaseCurrencyCode);
        Assert.NotNull(rate.TargetCurrencyCode);
        Assert.True(rate.Rate > 0);
        Assert.Equal(1, rate.Multiplier); // ECB always uses 1
    });
}
```

### Mock Response Test

```csharp
[Fact]
public async Task Converter_ParsesValidXml()
{
    // Arrange
    var xml = @"
    <gesmes:Envelope xmlns:gesmes='http://www.gesmes.org/xml/2002-08-01' xmlns='http://www.ecb.int/vocabulary/2002-08-01/eurofxref'>
      <Cube>
        <Cube time='2025-11-05'>
          <Cube currency='USD' rate='1.1492'/>
        </Cube>
      </Cube>
    </gesmes:Envelope>";

    var serializer = new XmlSerializer(typeof(EcbEnvelope));
    var ecbEnvelope = (EcbEnvelope)serializer.Deserialize(new StringReader(xml));

    var converter = new EcbConverter("EUR");

    // Act
    var rates = await converter.Convert(ecbEnvelope);

    // Assert
    Assert.Single(rates);
    Assert.Equal("EUR", rates[0].BaseCurrencyCode);
    Assert.Equal("USD", rates[0].TargetCurrencyCode);
    Assert.Equal(1.1492m, rates[0].Rate);
    Assert.Equal(1, rates[0].Multiplier);
}
```

## Troubleshooting

### Common Issues

**Issue: "ECB response Cube element is missing"**
- **Cause**: XML structure changed or network issue returned error page
- **Solution**: Check if ECB API is accessible, verify XML structure

**Issue: Timeout errors**
- **Cause**: Network latency, ECB server slow
- **Solution**: Increase timeout, implement retry logic

**Issue: XML parsing fails**
- **Cause**: Unexpected XML structure, namespace issues
- **Solution**: Verify XML namespaces match, check for BOM characters

**Issue: Missing currencies**
- **Cause**: ECB doesn't publish all currencies daily
- **Solution**: Check ECB's official currency list, use fallback provider

### Debug Mode

Enable detailed logging:

```csharp
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");

var provider = new EuropeanCentralBankProvider(options, httpClient);

// Log raw XML before processing
var response = await httpClient.GetStringAsync(options.Url);
Console.WriteLine($"Raw XML:\n{response}");
```

## References

- [ECB Official Website](https://www.ecb.europa.eu/)
- [ECB Exchange Rates Page](https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html)
- [ECB XML API Endpoint](https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml)
- [ECB 90-Day History](https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml)
- [ECB Full History](https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml)
- [ISO 4217 Currency Codes](https://www.iso.org/iso-4217-currency-codes.html)

## License

This provider implementation follows the same license as the parent project.

## Contributing

When contributing to this provider:

1. Maintain XML serialization compatibility
2. Follow error handling patterns
3. Add unit tests for new features
4. Update this README with changes
5. Test against live ECB API before committing
