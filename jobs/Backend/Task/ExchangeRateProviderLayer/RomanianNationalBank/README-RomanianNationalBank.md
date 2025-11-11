# Romanian National Bank (BNR) Exchange Rate Provider

Implementation of exchange rate provider for **Banca Națională a României** (Romanian National Bank).

## Overview

This provider fetches daily exchange rates from the Romanian National Bank's public XML API. The BNR publishes exchange rates for major currencies against the Romanian Leu (RON).

**Configuration is loaded from ConfigurationLayer** via `ExchangeRateProviderOptions`.

### Provider Information (from appsettings.json)
- **Name**: Banca Națională a României
- **Code**: BNR
- **Base Currency**: RON (Romanian Leu) - configurable
- **API Endpoint**: https://www.bnr.ro/nbrfxrates.xml - configurable
- **Data Format**: XML
- **Update Frequency**: Daily (typically updated around 13:00 EET)
- **Authentication**: Not required (public API)
- **Rate Limit**: None specified
- **Historical Data**: Limited (main endpoint provides latest rates only)

## Features

✅ Fetches latest daily exchange rates
✅ XML parsing with automatic deserialization
✅ Comprehensive error handling
✅ Type-safe data models
✅ Standardized DTO conversion
✅ Timeout protection (30 seconds)
⚠️ Historical data limited to latest available rates

## Implementation

### Architecture

```
RomanianNationalBank/
├── Models/
│   └── BnrDataSet.cs           # XML response models
├── Converters/
│   └── BnrConverter.cs         # Converts BNR response to ExchangeRateDTO
├── RomanianNationalBankProvider.cs  # Main provider implementation
└── README.md
```

### Components

#### 1. **BnrDataSet** (Models)

XML response models matching the BNR API structure:

```csharp
[XmlRoot("DataSet", Namespace = "http://www.bnr.ro/xsd")]
public class BnrDataSet
{
    [XmlElement("Body")]
    public BnrBody? Body { get; set; }
}

public class BnrCube
{
    [XmlAttribute("date")]
    public string? Date { get; set; }

    [XmlElement("Rate")]
    public List<BnrRate> Rates { get; set; }
}

public class BnrRate
{
    [XmlAttribute("currency")]
    public string? Currency { get; set; }

    [XmlAttribute("multiplier")]
    public int Multiplier { get; set; }

    [XmlText]
    public decimal Value { get; set; }
}
```

#### 2. **BnrConverter**

Implements `IExchangeRateConverter<BnrDataSet>` to transform BNR's XML response into standardized `ExchangeRateDTO` objects.

**Conversion Logic:**
- Base currency: Always RON
- Target currency: From XML `currency` attribute
- Rate: Direct value from XML
- Multiplier: From XML (usually 1)

**Validation:**
- Skips rates with missing/invalid currency codes
- Skips rates with zero or negative values
- Throws if no valid rates found

#### 3. **RomanianNationalBankProvider**

Main provider implementing `IExchangeRateProvider`.

**Key Features:**
- Async HTTP requests with 30-second timeout
- XML deserialization using `XmlSerializer`
- Comprehensive exception handling
- Standardized status codes and messages

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
VALUES ('Banca Națională a României', 'BNR', 'https://www.bnr.ro/nbrfxrates.xml', @RonId, 1, 0);
```

**ExchangeRateProviderConfiguration table:**
```sql
INSERT INTO ExchangeRateProviderConfiguration (ProviderId, SettingKey, SettingValue, Description)
VALUES
    (@BnrId, 'Format', 'XML', 'Response format'),
    (@BnrId, 'XmlNamespace', 'http://www.bnr.ro/xsd', 'BNR XML namespace'),
    (@BnrId, 'UpdateTime', '13:00', 'Daily update time'),
    (@BnrId, 'TimeZone', 'EET', 'Eastern European Time'),
    (@BnrId, 'HistoricalUrl', 'https://www.bnr.ro/files/xml/years/nbrfxrates{year}.xml', 'Historical rates by year');
```

See `Database/Script/SeedExchangeRateProvidersAndConfiguration.sql` for complete seed data.

#### Fallback Configuration (appsettings.json)

```json
{
  "ExchangeRateProviders": {
    "BNR": {
      "Name": "Banca Națională a României",
      "Code": "BNR",
      "Url": "https://www.bnr.ro/nbrfxrates.xml",
      "BaseCurrency": "RON",
      "IsActive": true,
      "RequiresAuthentication": false,
      "Configuration": {
        "Format": "XML",
        "DecimalSeparator": "Dot",
        "UpdateTime": "13:00",
        "TimeZone": "EET",
        "XmlNamespace": "http://www.bnr.ro/xsd",
        "HasVariableAmounts": true,
        "XmlStructure": "DataSet/Body/Cube/Rate",
        "HasHeader": true,
        "HistoricalUrl": "https://www.bnr.ro/files/xml/years/nbrfxrates{year}.xml"
      }
    }
  }
}
```

### Basic Usage (Recommended)

**Using IProviderConfigurationService (Database-first):**

```csharp
using RomanianNationalBank;
using ConfigurationLayer.Interface;

// Inject IProviderConfigurationService
private readonly IProviderConfigurationService _providerConfig;
private readonly IHttpClientFactory _httpClientFactory;

public async Task FetchBnrRates()
{
    // Load configuration from database (or fallback to appsettings.json)
    var bnrOptions = await _providerConfig.GetProviderConfigurationAsync("BNR");

    if (bnrOptions == null)
    {
        Console.WriteLine("BNR provider not configured");
        return;
    }

    // Create provider with configuration
    var httpClient = _httpClientFactory.CreateClient();
    var provider = new RomanianNationalBankProvider(bnrOptions, httpClient);

    // Fetch today's rates
    var (status, rates) = await provider.GetExchangeRatesForToday();

    // Check status
    if (status.Item1 == 200) // HTTP 200 OK
    {
        Console.WriteLine($"Success: {status.Item2}");

        foreach (var rate in rates)
        {
            Console.WriteLine($"1 {rate.BaseCurrencyCode} = {rate.Rate / rate.Multiplier} {rate.TargetCurrencyCode}");
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

    public async Task FetchBnrRates()
    {
        // Load configuration from database (with appsettings.json fallback)
        var bnrOptions = await _providerConfig.GetProviderConfigurationAsync("BNR");

        if (bnrOptions == null || !bnrOptions.IsActive)
        {
            _logger.LogWarning("BNR provider is not configured or not active");
            return;
        }

        // Create provider with loaded configuration
        var httpClient = _httpClientFactory.CreateClient();
        var provider = new RomanianNationalBankProvider(bnrOptions, httpClient);

        // Fetch rates
        var (status, rates) = await provider.GetExchangeRatesForToday();

        if (status.Item1 == 200)
        {
            _logger.LogInformation("Successfully fetched {Count} exchange rates from BNR", rates.Count);
            // Process rates...
        }
        else
        {
            _logger.LogError("Failed to fetch BNR rates: {StatusCode} - {Message}",
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
            if (providerOptions.Code == "BNR")
            {
                var httpClient = _httpClientFactory.CreateClient();
                var provider = new RomanianNationalBankProvider(providerOptions, httpClient);
                var (status, rates) = await provider.GetExchangeRatesForToday();
                // Process rates...
            }
            // Handle other providers (CNB, ECB, etc.)
        }
    }
}
```

### Configuration Priority Flow

```
┌─────────────────────────────────────────┐
│ IProviderConfigurationService Request   │
│ GetProviderConfigurationAsync("BNR")    │
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

### Legacy Usage (Direct appsettings.json)

**Note:** This approach bypasses the database and should only be used for testing or when database is unavailable.

```csharp
// Load configuration directly from appsettings.json
var bnrOptions = configuration.GetSection("ExchangeRateProviders:BNR")
    .Get<ExchangeRateProviderOptions>();

// Create provider with custom HttpClient
var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(60),
    DefaultRequestHeaders =
    {
        { "User-Agent", "ExchangeRateApp/1.0" }
    }
};

var provider = new RomanianNationalBankProvider(bnrOptions, httpClient);
var (status, rates) = await provider.GetExchangeRatesForToday();
```

## API Response Example

### XML Response from BNR

```xml
<?xml version="1.0" encoding="utf-8"?>
<DataSet xmlns="http://www.bnr.ro/xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Body>
    <Cube date="2025-11-05">
      <Rate currency="AED" multiplier="1">1.2345</Rate>
      <Rate currency="USD" multiplier="1">4.5678</Rate>
      <Rate currency="EUR" multiplier="1">4.9876</Rate>
      <Rate currency="GBP" multiplier="1">5.7890</Rate>
      <Rate currency="CHF" multiplier="1">5.1234</Rate>
      <!-- ... more rates ... -->
    </Cube>
  </Body>
</DataSet>
```

### Converted Output

```csharp
[
  {
    BaseCurrencyCode = "RON",
    TargetCurrencyCode = "AED",
    Multiplier = 1,
    Rate = 1.2345
  },
  {
    BaseCurrencyCode = "RON",
    TargetCurrencyCode = "USD",
    Multiplier = 1,
    Rate = 4.5678
  },
  // ... more rates
]
```

## Rate Interpretation

BNR rates represent: **1 [Foreign Currency] = [Rate] RON**

### Examples:

**XML Rate:**
```xml
<Rate currency="USD" multiplier="1">4.5678</Rate>
```

**Interpretation:**
- 1 USD = 4.5678 RON
- To convert RON to USD: divide by rate
- To convert USD to RON: multiply by rate

**With Multiplier:**
```xml
<Rate currency="HUF" multiplier="100">1.2345</Rate>
```

**Interpretation:**
- 100 HUF = 1.2345 RON
- 1 HUF = 0.012345 RON
- Effective rate = 1.2345 / 100

## Status Codes

The provider returns standardized HTTP-like status codes:

| Code | Meaning | Description |
|------|---------|-------------|
| 200 | Success | Rates fetched successfully |
| 408 | Timeout | Request timed out (> 30 seconds) |
| 500 | Server Error | XML parsing or validation error |
| 503 | Service Unavailable | Network error, BNR API unreachable |
| Other | HTTP Status | Actual HTTP status from BNR API |

### Example Status Messages

```csharp
// Success
(200, "Romanian National Bank (BNR): Successfully retrieved 42 exchange rates")

// Network error
(503, "Romanian National Bank (BNR): Network error - No such host is known")

// Timeout
(408, "Romanian National Bank (BNR): Request timeout - The operation was canceled")

// Validation error
(500, "Romanian National Bank (BNR): Data validation error - No valid exchange rates found")
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

The main BNR API endpoint (`nbrfxrates.xml`) only provides the latest daily rates. For historical data:

### Current Implementation
```csharp
var (status, rates) = await provider.GetHistoryExchangeRates();
// Returns: Latest available rates with note in message
```

### Future Enhancement Options

1. **BNR Archive Files**
   - BNR provides downloadable CSV/XML files with historical data
   - URL pattern: `https://www.bnr.ro/files/xml/years/nbrfxrates{YEAR}.xml`

2. **Database Caching**
   - Cache daily rates in local database
   - Build historical dataset over time

3. **Date Range Parameters**
   - Extend interface to accept date range
   - Fetch and aggregate from archive files

**Example Future Implementation:**
```csharp
public async Task<List<ExchangeRateDTO>> GetHistoryExchangeRates(
    DateTime startDate,
    DateTime endDate)
{
    // Fetch from archive files or local cache
}
```

## Supported Currencies

The BNR typically publishes rates for 40+ currencies including:

**Major Currencies:**
- USD (US Dollar)
- EUR (Euro)
- GBP (British Pound)
- CHF (Swiss Franc)
- JPY (Japanese Yen)

**Regional:**
- HUF (Hungarian Forint)
- CZK (Czech Koruna)
- PLN (Polish Zloty)
- BGN (Bulgarian Lev)
- HRK (Croatian Kuna)

**Others:**
- AED, AUD, BRL, CAD, CNY, DKK, EGP, INR, KRW, MXN, NOK, NZD, RUB, SEK, SGD, THB, TRY, ZAR, and more

**Note:** Currency availability may change based on BNR policy.

## Performance Considerations

### Caching Recommendations

Since BNR rates update once daily (around 13:00 CET), caching is highly recommended:

```csharp
public class CachedBnrProvider
{
    private readonly RomanianNationalBankProvider _provider;
    private readonly IMemoryCache _cache;

    public async Task<List<ExchangeRateDTO>> GetCachedRates()
    {
        var cacheKey = $"BNR_Rates_{DateTime.UtcNow:yyyy-MM-dd}";

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
    var provider = new RomanianNationalBankProvider();

    // Act
    var (status, rates) = await provider.GetExchangeRatesForToday();

    // Assert
    Assert.Equal(200, status.Item1);
    Assert.NotEmpty(rates);
    Assert.All(rates, rate =>
    {
        Assert.Equal("RON", rate.BaseCurrencyCode);
        Assert.NotNull(rate.TargetCurrencyCode);
        Assert.True(rate.Rate > 0);
        Assert.True(rate.Multiplier > 0);
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
    <DataSet xmlns='http://www.bnr.ro/xsd'>
      <Body>
        <Cube date='2025-11-05'>
          <Rate currency='USD' multiplier='1'>4.5678</Rate>
        </Cube>
      </Body>
    </DataSet>";

    var serializer = new XmlSerializer(typeof(BnrDataSet));
    var bnrDataSet = (BnrDataSet)serializer.Deserialize(
        new StringReader(xml));

    var converter = new BnrConverter();

    // Act
    var rates = await converter.Convert(bnrDataSet);

    // Assert
    Assert.Single(rates);
    Assert.Equal("RON", rates[0].BaseCurrencyCode);
    Assert.Equal("USD", rates[0].TargetCurrencyCode);
    Assert.Equal(4.5678m, rates[0].Rate);
}
```

## Troubleshooting

### Common Issues

**Issue: "No valid exchange rates found"**
- **Cause**: BNR response structure changed or empty response
- **Solution**: Check XML response format, verify API is accessible

**Issue: Timeout errors**
- **Cause**: Network latency, BNR server slow
- **Solution**: Increase timeout, implement retry logic

**Issue: XML parsing fails**
- **Cause**: Unexpected XML structure, encoding issues
- **Solution**: Verify XML namespace matches, check for BOM characters

**Issue: Missing currencies**
- **Cause**: BNR doesn't publish rates for all currencies daily
- **Solution**: Check BNR's official currency list, use fallback provider

### Debug Mode

Enable detailed logging:

```csharp
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");

var provider = new RomanianNationalBankProvider(httpClient);

// Log raw XML before processing
var response = await httpClient.GetStringAsync(BnrApiUrl);
Console.WriteLine($"Raw XML:\n{response}");
```

## References

- [BNR Official Website](https://www.bnr.ro/)
- [BNR Exchange Rates Page](https://www.bnr.ro/Exchange-rates-15192.aspx)
- [BNR XML API Endpoint](https://www.bnr.ro/nbrfxrates.xml)
- [ISO 4217 Currency Codes](https://www.iso.org/iso-4217-currency-codes.html)

## License

This provider implementation follows the same license as the parent project.

## Contributing

When contributing to this provider:

1. Maintain XML serialization compatibility
2. Follow error handling patterns
3. Add unit tests for new features
4. Update this README with changes
5. Test against live BNR API before committing
