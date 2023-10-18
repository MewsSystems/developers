using Mews.ExchangeRateProvider.Extensions;
using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System.Net.Mime;
using System.Reflection;

namespace Mews.ExchangeRateProvider.Tests;

[TestFixture]
public sealed class CzechNationalBankExchangeRateProviderTests
{
    private static string TestData { get; set; }
    private static string TestData2 { get; set; }

    [OneTimeSetUp]
    public static void Init()
    {
        var testDataFilePath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath)!, "TestData", "ExampleData.txt");
        TestData = File.ReadAllText(testDataFilePath);

        var testDataFilePath2 = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath)!, "TestData", "ExampleData2.txt");
        TestData2 = File.ReadAllText(testDataFilePath2);
    }

    [Test]
    public async Task CzechNationalBankExchangeRateProvider_returns_single_currency()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions());

        var results = (await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None)).ToList();

        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results.Single().SourceCurrency.Code, Is.EqualTo("AUD"));
        Assert.That(results.Single().TargetCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(results.Single().Value, Is.EqualTo(14.805m));
    }

    [Test]
    public async Task CzechNationalBankExchangeRateProvider_returns_currencies_from_both_files()
    {
        var sut = new CzechNationalBankExchangeRateProvider(CreateStubbedHttpClient(), new CzechNationalBankExchangeRateMapper(), CreateOptions());

        var results = (await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD"), new("CUP") }, CancellationToken.None)).ToList();

        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("AUD")).TargetCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("AUD")).Value, Is.EqualTo(14.805m));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("CUP")).TargetCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(results.Single(r => r.SourceCurrency.Code.Equals("CUP")).Value, Is.EqualTo(0.957m));
    }
    
    //TODO:
    //Mock exception on calling one or both of the http get requests
    //Mock one or both unsuccessful status code replies on http get
    //Mock exception thrown by mapper
    //Empty reply generated for non-existent currency

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
}