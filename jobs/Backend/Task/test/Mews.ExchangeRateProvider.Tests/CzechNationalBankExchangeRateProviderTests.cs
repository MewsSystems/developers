using Mews.ExchangeRateProvider.Exceptions;
using Mews.ExchangeRateProvider.Extensions;
using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Mime;
using System.Reflection;

namespace Mews.ExchangeRateProvider.Tests;

[TestFixture]
public sealed class CzechNationalBankExchangeRateProviderTests
{
    private static string TestData { get; set; }
    private static string TestData2 { get; set; }

    private static ILogger<CzechNationalBankExchangeRateProvider> Logger { get; set; }

    [OneTimeSetUp]
    public static void Init()
    {
        var testDataFilePath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath)!, "TestData", "ExampleData.txt");
        TestData = File.ReadAllText(testDataFilePath);

        var testDataFilePath2 = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath)!, "TestData", "ExampleData2.txt");
        TestData2 = File.ReadAllText(testDataFilePath2);

        Logger = NullLogger<CzechNationalBankExchangeRateProvider>.Instance;
    }

    [Test]
    public async Task CzechNationalBankExchangeRateProvider_returns_empty_if_currency_not_known()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        var results = (await sut.GetExchangeRatesAsync(new List<Currency> { new("000") }, CancellationToken.None)).ToList();

        Assert.That(results.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task CzechNationalBankExchangeRateProvider_returns_single_currency()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        var results = (await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None)).ToList();

        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results.Single().SourceCurrency.Code, Is.EqualTo("AUD"));
        Assert.That(results.Single().TargetCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(results.Single().Value, Is.EqualTo(14.805m));
    }

    [Test]
    public async Task CzechNationalBankExchangeRateProvider_returns_currencies_from_both_files()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        var results = (await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD"), new("CUP") }, CancellationToken.None)).ToList();

        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("AUD")).TargetCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("AUD")).Value, Is.EqualTo(14.805m));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("CUP")).TargetCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("CUP")).Value, Is.EqualTo(0.957m));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_argument_null_exception_if_currency_null()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetExchangeRatesAsync(null!, CancellationToken.None));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_argument_exception_if_currency_list_empty()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetExchangeRatesAsync(Enumerable.Empty<Currency>(), CancellationToken.None));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_invalid_operation_exception_if_options_null()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), new NullOptions(), Logger);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_invalid_operation_exception_if_options_uri_null()
    {
        var options = Options.Create(new CzechNationalBankExchangeRateProviderOptions
        {
            ExchangeRateProviders = new List<CzechNationalBankExchangeRateProviderConfiguration>
            {
                new() { Uri = null }
            }
        });

        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), options, Logger);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_obtain_exchange_rate_exception_if_response_malformed()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp
            .When("http://test.com/a.txt")
            .Respond(MediaTypeNames.Text.Plain, "\n\n\n\ndasfasu32222222||||ASADF|ADSF||||||||||||asjdfkadjklfkljdjlfjklajklfdjklasdjklfjkladsjklafjdfjkfkjljklajklfajk\nadsfjdklfjkl}}}");

        mockHttp
            .When("http://test.com/b.txt")
            .Respond(MediaTypeNames.Text.Plain, TestData2);

        var sut = new CzechNationalBankExchangeRateProvider(new HttpClient(mockHttp), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        Assert.ThrowsAsync<ObtainExchangeRateException>(async () => await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_obtain_exchange_rate_exception_if_remote_status_code_unsuccessful()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp
            .When("http://test.com/a.txt")
            .Respond(HttpStatusCode.InternalServerError);

        mockHttp
            .When("http://test.com/b.txt")
            .Respond(MediaTypeNames.Text.Plain, TestData2);

        var sut = new CzechNationalBankExchangeRateProvider(new HttpClient(mockHttp), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        Assert.ThrowsAsync<ObtainExchangeRateException>(async () => await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None));
    }

    [Test]
    public void CzechNationalBankExchangeRateProvider_throws_obtain_exchange_rate_exception_if_http_exception_thrown()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp
            .When("http://test.com/a.txt")
            .Throw(new HttpRequestException("Error"));

        mockHttp
            .When("http://test.com/b.txt")
            .Respond(MediaTypeNames.Text.Plain, TestData2);

        var sut = new CzechNationalBankExchangeRateProvider(new HttpClient(mockHttp), new CzechNationalBankExchangeRateMapper(), CreateOptions(), Logger);

        Assert.ThrowsAsync<ObtainExchangeRateException>(async () => await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None));
    }

    private static IOptions<CzechNationalBankExchangeRateProviderOptions> CreateOptions() =>
        Options.Create(new CzechNationalBankExchangeRateProviderOptions
        {
            ExchangeRateProviders = new List<CzechNationalBankExchangeRateProviderConfiguration>
            {
                new() { Uri = new Uri("http://test.com/a.txt") },
                new() { Uri = new Uri("http://test.com/b.txt") }
            }
        });

    private static HttpClient CreateStubbedHttpClient()
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp
            .When("http://test.com/a.txt")
            .Respond(MediaTypeNames.Text.Plain, TestData);

        mockHttp
            .When("http://test.com/b.txt")
            .Respond(MediaTypeNames.Text.Plain, TestData2);

        return new HttpClient(mockHttp);
    }

    private class NullOptions : IOptions<CzechNationalBankExchangeRateProviderOptions>
    {
        public CzechNationalBankExchangeRateProviderOptions Value => null!;
    }
}