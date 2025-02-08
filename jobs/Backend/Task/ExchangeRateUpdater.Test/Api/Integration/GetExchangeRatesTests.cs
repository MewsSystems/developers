using System.Net;
using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Contract;
using ExchangeRateUpdater.Contract.ExchangeRate;
using FuncSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NFluent;
using NSubstitute;

namespace ExchangeRateUpdater.Test.Api.Integration;

public class GetExchangeRatesTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var fetcher = Substitute.For<ICnbExchangeRateFetcher>();

                    var cnbRates = new List<CnbExchangeRate>
                    {
                        new(Order: 94, Country: "USA", Currency: "dollar", CurrencyCode: Currency.Usd, Amount: 1,
                            Rate: 23.048m),
                        new(Order: 94, Country: "EMU", Currency: "euro", CurrencyCode: Currency.Eur, Amount: 1,
                            Rate: 25.75m),
                        new(Order: 94, Country: "Filipíny", Currency: "peso", CurrencyCode: Currency.Php, Amount: 100,
                            Rate: 43.71m)
                    };

                    fetcher.FetchExchangeRatesAsync(Arg.Any<CancellationToken>())
                        .Returns(Try.Success<IEnumerable<CnbExchangeRate>, CnbExchangeRatesFetchError>(cnbRates));

                    services.AddScoped<ICnbExchangeRateFetcher>(_ => fetcher);
                });
            });

        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnSuccess_WhenValidCurrencies()
    {
        // Arrange
        const string currencies = "USD,EUR";
        const string requestUri = $"/exchange-rates?currencies={currencies}";

        // Act
        var response = await _client.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode(); // Assert 2xx status code
        var content = await response.Content.ReadAsStringAsync();

        var expected = await LoadExpectedResponseAsync("ShouldReturnSuccess_WhenValidCurrencies");

        Check.That(content).IsEqualTo(expected);
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnBadRequest_WhenCurrenciesNotProvided()
    {
        // Arrange
        const string requestUri = "/exchange-rates";

        // Act
        var response = await _client.GetAsync(requestUri);

        // Assert
        Check.That(response.StatusCode).Is(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        var expected = await LoadExpectedResponseAsync("ShouldReturnBadRequest_WhenCurrenciesNotProvided");

        Check.That(content).IsEqualTo(expected);
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnBadRequest_WhenCurrenciesInvalid()
    {
        // Arrange
        const string currencies = "INVALID";
        const string requestUri = $"/exchange-rates?currencies={currencies}";

        // Act
        var response = await _client.GetAsync(requestUri);

        // Assert
        Check.That(response.StatusCode).Is(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        var expected = await LoadExpectedResponseAsync("ShouldReturnBadRequest_WhenCurrenciesInvalid");

        Check.That(content).IsEqualTo(expected);
    }

    private static async Task<string> LoadExpectedResponseAsync(string fileName)
    {
        var filePath = @$"..\..\..\Api\Integration\Data\{fileName}.txt";

        return await File.ReadAllTextAsync(filePath);
    }
}