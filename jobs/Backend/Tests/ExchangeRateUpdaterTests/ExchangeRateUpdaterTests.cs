using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ExchangeRateUpdaterTests;

public class ExchangeRateUpdaterTests
{
    private const string UrlEndpoint = "/GetRates";
    private WireMockServer? _server;
    private ExchangeRateProviderSettings? _settings;
    private HttpClient? _httpClient;
    private ExchangeRateProvider _provider;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _server = WireMockServer.Start();
        _settings = new ExchangeRateProviderSettings { BankUrl = $"{_server.Url}{UrlEndpoint}" };
        _httpClient = new HttpClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _server?.Stop();
    }

    [SetUp]
    public void Setup()
    {
        _server!.Reset();
        _provider = new ExchangeRateProvider(_httpClient!, Options.Create(_settings!));
    }
    
    [Test]
    public async Task GetExchangeRatesAsync_WhenPassingAnEmptySetOfCurrencies_ShouldReturnEmptyResult()
    {
        // Actions
        var result = await _provider.GetExchangeRatesAsync(new HashSet<Currency>());
        
        // Post-Conditions
        Assert.That(result, Is.Empty);
        AssertNoServerRequestsWereSent();
    }

    [Test]
    public async Task GetExchangeRatesAsync_WhenPassingValidSetOfCurrencies_ShouldReturnRates()
    {
        // Pre-Conditions
        GetRatesWithSuccess(GetDefaultResponseBody());
        var targetConcurrency = new Currency("CZK");
        var expectedResult = new List<ExchangeRate>
        {
            new(new Currency("EUR"), targetConcurrency, 24.645m),
            new(new Currency("JPY"), targetConcurrency, 0.14282m),
            new(new Currency("THB"), targetConcurrency, 0.65269m),
            new(new Currency("TRY"), targetConcurrency, 0.52843m),
            new(new Currency("USD"), targetConcurrency, 21.248m),
        };
        
        // Actions
        var result = await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList());
        
        // Post-Conditions
        AssertResultIsAsExpected(result, expectedResult);
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public async Task GetExchangeRatesAsync_WhenNoMatchingCurrencies_ReturnEmptyResult()
    {
        // Pre-Conditions
        GetRatesWithSuccess(GetDefaultResponseBody());
        var currencies = new HashSet<Currency>
            {
                new("CZK"),
                new("KES"),
                new("RUB"),
                new("XYZ")
            };
        
        // Actions
        var result = await _provider.GetExchangeRatesAsync(currencies);
        
        // Post-Conditions
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public void GetExchangeRatesAsync_WhenEndpointNotFound_ThrowException()
    {
        // Pre-Conditions
        GetRatesWithNotFound();
        
        // Action
        var exception = Assert.ThrowsAsync<HttpRequestException>(async () => await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList()));
        
        // Post-Conditions
        Assert.That(exception!.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public void GetExchangeRatesAsync_WhenMissingHeader_ThrowException()
    {
        // Pre-Conditions
        var responseBody = "16 Jul 2025 #136";

        GetRatesWithSuccess(responseBody);
        
        // Action
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList()));
        
        // Post-Conditions
        Assert.That(exception!.Message, Is.EqualTo("Missing header line"));
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public void GetExchangeRatesAsync_WhenInvalidHeader_ThrowException()
    {
        // Pre-Conditions
        var responseBody = @"16 Jul 2025 #136
Currency|Country|Amount|Code|Rate
dollar|Australia|1|AUD|13.850";

        GetRatesWithSuccess(responseBody);
        
        // Action
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList()));
        
        // Post-Conditions
        Assert.That(exception!.Message, Is.EqualTo("Invalid header line. Expected: 'Country|Currency|Amount|Code|Rate' Got: 'Currency|Country|Amount|Code|Rate'"));
        AssertServerRequestsWereSentOnce();
    }

    [TestCase("Australia|1|AUD|13.850")]
    [TestCase("Australia|dollar|1|AUD|13.850|ExtraValue")]
    public void GetExchangeRatesAsync_WhenLineHasWrongFormat_ThrowException(string line)
    {
        // Pre-Conditions
        var responseBody = @$"16 Jul 2025 #136
Country|Currency|Amount|Code|Rate
{line}";

        GetRatesWithSuccess(responseBody);
        
        // Action
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList()));
        
        // Post-Conditions
        Assert.That(exception!.Message, Is.EqualTo($"Invalid number of parts on line: {line}"));
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public void GetExchangeRatesAsync_WhenRateNotANumber_ThrowException()
    {
        // Pre-Conditions
        var responseBody = @"16 Jul 2025 #136
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|nan";

        GetRatesWithSuccess(responseBody);
        
        // Action
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList()));
        
        // Post-Conditions
        Assert.That(exception!.Message, Is.EqualTo("Unable to parse rate for line: USA|dollar|1|USD|nan"));
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public void GetExchangeRatesAsync_WhenAmountNotANumber_ThrowException()
    {
        // Pre-Conditions
        var responseBody = @"16 Jul 2025 #136
Country|Currency|Amount|Code|Rate
USA|dollar|nan|USD|123";

        GetRatesWithSuccess(responseBody);
        
        // Action
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList()));
        
        // Post-Conditions
        Assert.That(exception!.Message, Is.EqualTo("Unable to parse amount for line: USA|dollar|nan|USD|123"));
        AssertServerRequestsWereSentOnce();
    }

    [Test]
    public async Task GetExchangeRatesAsync_WhenReturningMultipleRatesForSameCurrency_ExpectMultipleResults()
    {
        // Pre-Conditions
        var responseBody = @"16 Jul 2025 #136
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|100
USA|dollar|2|USD|100";
        GetRatesWithSuccess(responseBody);
        var targetConcurrency = new Currency("CZK");
        var expectedResult = new List<ExchangeRate>
        {
            new(new Currency("USD"), targetConcurrency, 100m),
            new(new Currency("USD"), targetConcurrency, 50m),
        };
        
        // Action
        var result = await _provider.GetExchangeRatesAsync(GetDefaultCurrencyList());
        
        // Post-Conditions
        AssertResultIsAsExpected(result, expectedResult);
        AssertServerRequestsWereSentOnce();
    }
    
    private static string GetDefaultResponseBody()
    {
        return @"16 Jul 2025 #136
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|13.850
Brazil|real|1|BRL|3.818
Bulgaria|lev|1|BGN|12.602
Canada|dollar|1|CAD|15.479
China|renminbi|1|CNY|2.959
Denmark|krone|1|DKK|3.302
EMU|euro|1|EUR|24.645
Hongkong|dollar|1|HKD|2.707
Hungary|forint|100|HUF|6.162
Iceland|krona|100|ISK|17.331
IMF|SDR|1|XDR|29.067
India|rupee|100|INR|24.711
Indonesia|rupiah|1000|IDR|1.306
Israel|new shekel|1|ILS|6.328
Japan|yen|100|JPY|14.282
Malaysia|ringgit|1|MYR|5.006
Mexico|peso|1|MXN|1.129
New Zealand|dollar|1|NZD|12.618
Norway|krone|1|NOK|2.062
Philippines|peso|100|PHP|37.215
Poland|zloty|1|PLN|5.787
Romania|leu|1|RON|4.858
Singapore|dollar|1|SGD|16.531
South Africa|rand|1|ZAR|1.186
South Korea|won|100|KRW|1.527
Sweden|krona|1|SEK|2.177
Switzerland|franc|1|CHF|26.420
Thailand|baht|100|THB|65.269
Turkey|lira|100|TRY|52.843
United Kingdom|pound|1|GBP|28.460
USA|dollar|1|USD|21.248
";
    }

    private static HashSet<Currency> GetDefaultCurrencyList()
    {
        return new HashSet<Currency>
        {
            new("USD"),
            new("EUR"),
            new("CZK"),
            new("JPY"),
            new("KES"),
            new("RUB"),
            new("THB"),
            new("TRY"),
            new("XYZ")
        };
    }

    private void GetRatesWithSuccess(string bodyContent)
    {
        var serverResponse = Response.Create().WithSuccess().WithBody(bodyContent);
        var serverRequest = Request.Create().WithPath(UrlEndpoint).UsingMethod(HttpMethod.Get.ToString());
        _server!.Given(serverRequest).RespondWith(serverResponse);
    }
    
    private void GetRatesWithNotFound()
    {
        var serverResponse = Response.Create().WithNotFound();
        var serverRequest = Request.Create().WithPath(UrlEndpoint).UsingMethod(HttpMethod.Get.ToString());
        _server!.Given(serverRequest).RespondWith(serverResponse);
    }

    private void AssertNoServerRequestsWereSent()
    {
        Assert.That(_server!.LogEntries.Count, Is.EqualTo(0));
    }
    
    private void AssertServerRequestsWereSentOnce()
    {
        Assert.That(_server!.LogEntries.Count, Is.EqualTo(1));
    }

    private void AssertResultIsAsExpected(IReadOnlyList<ExchangeRate> result, IReadOnlyList<ExchangeRate> expectedResult)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        for (var i = 0; i < result.Count; i++)
        {
            Assert.That(result[i].ToString(), Is.EqualTo(expectedResult[i].ToString()));
        }
    }
}