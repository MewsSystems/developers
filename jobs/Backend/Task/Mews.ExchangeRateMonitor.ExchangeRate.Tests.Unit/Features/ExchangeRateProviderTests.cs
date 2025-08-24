using FluentAssertions;
using Mews.ExchangeRateMonitor.ExchangeRate.Domain;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Features;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Options;
using Mews.ExchangeRateMonitor.ExchangeRate.Features.Shared.HttpClient;
using Mews.ExchangeRateMonitor.ExchangeRate.Tests.Unit.TestHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Net;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Tests.Unit.Features;

public class ExchangeRateProviderTests
{
    private readonly IEnumerable<string> _requiredCurrencies = ["USD", "EUR"];
    private (int CurAmount, string Json) ReturnedJsonWithCurAmount => (3, """
               {"rates":[
                   {"currencyCode":"USD","amount":1,"rate":22.00},
                   {"currencyCode":"EUR","amount":1,"rate":25.00},
                   {"currencyCode":"PLN","amount":1,"rate":5.00}
               ]}
               """);

    private readonly DateOnly _date1 = new DateOnly(2024, 12, 24);

    private static IOptions<ExchangeRateModuleOptions> Opts(IEnumerable<string> reqCurs) =>
        Options.Create(
            new ExchangeRateModuleOptions
            {
                CnbExratesOptions = new CnbExratesOptions()
                {
                    BaseCnbApiUri = "https://cnb.local/",
                    RequiredCurrencies = reqCurs
                }
            });

    private static IHttpClientFactory HttpFactory(HttpMessageHandler handler)
    {
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://cnb.local/") };
        var f = Substitute.For<IHttpClientFactory>();
        f.CreateClient(HttpClientConsts.HttpCnbClient).Returns(client);
        return f;
    }

    [Fact]
    public async Task Returns_Value_From_HttpClient_When_There_Is_No_Chache()
    {
        var handler = new StaticResponseHandler(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(ReturnedJsonWithCurAmount.Json, System.Text.Encoding.UTF8, "application/json")
            });

        var cache = Substitute.For<IMemoryCache>();
        var logger = Substitute.For<ILogger<ExchangeRateProvider>>();
        var exchangeRateProvider = new ExchangeRateProvider(logger, Opts(_requiredCurrencies), cache, HttpFactory(handler));

        var res = await exchangeRateProvider.GetDailyRatesAsync(_date1, default);

        res.IsError.Should().BeFalse();
        res.Value!.Count().Should().Be(_requiredCurrencies.Count());
    }

    [Fact]
    public async Task Returns_Cached_Value_And_Skips_Http_On_Cache_Hit()
    {
        var key = $"cnb:daily:{_date1:yyyy-MM-dd}";
        var cached = new List<CurrencyExchangeRate> { new(new("CNY"), new("CZK"), 1m, 3.334m) };

        var cache = Substitute.For<IMemoryCache>();
        object boxed = cached;
        cache.TryGetValue(key, out Arg.Any<object?>()).Returns(ci => { ci[1] = boxed; return true; });

        var logger = Substitute.For<ILogger<ExchangeRateProvider>>();
        var handler = new StaticResponseHandler(CreateResponseMessage(HttpStatusCode.OK, """{"rates":null}"""));
        var httpFactory = HttpFactory(handler);
        var exchangeRateProvider = new ExchangeRateProvider(logger, Opts(_requiredCurrencies), cache, httpFactory);

        var res = await exchangeRateProvider.GetDailyRatesAsync(_date1, default);

        res.IsError.Should().BeFalse();
        res.Value.Should().BeEquivalentTo(cached);
    }

    [Fact]
    public async Task Returns_Failure_When_Cnb_Response_Missing_Rates()
    {
        var handler = new StaticResponseHandler(CreateResponseMessage(HttpStatusCode.OK, """{"rates":null}"""));
        var cache = Substitute.For<IMemoryCache>();
        cache.TryGetValue(Arg.Any<object>(), out Arg.Any<object?>()).Returns(ci => { ci[1] = null; return false; });

        var logger = Substitute.For<ILogger<ExchangeRateProvider>>();
        var exchangeRateProvider = new ExchangeRateProvider(logger, Opts(_requiredCurrencies), cache, HttpFactory(handler));

        var res = await exchangeRateProvider.GetDailyRatesAsync(_date1, default);

        res.IsError.Should().BeTrue();
    }

    private HttpResponseMessage CreateResponseMessage(HttpStatusCode code, string returnedJson)
    {
        return new HttpResponseMessage(code)
        {
            Content = new StringContent(returnedJson, System.Text.Encoding.UTF8, "application/json"),
        };
    }
}
