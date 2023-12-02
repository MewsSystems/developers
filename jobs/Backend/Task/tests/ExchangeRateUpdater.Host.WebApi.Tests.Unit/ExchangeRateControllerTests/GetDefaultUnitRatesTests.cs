using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using Flurl;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.ExchangeRateControllerTests;

[TestFixture]
internal class GetDefaultUnitRatesTests
{
    private IHost? _host;
    private TestServer _server;
    private HttpClient _client;
    private const string ApiBaseAddress = "http://exchange-rate-update.com";
    private ExchangeRateProviderRepositoryInMemory ExchangeRateProviderRepository;

    [Test]
    public async Task GivenNoDefaultUnitRatesStored_WhenQueryingGetDefaultUnitRates_ShouldReturnEmptyList()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRate").AppendPathSegment("defaultRates");
        var response = await _client.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRate>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSeveralUnitRatesStored_WhenQueryingGetDefaultUnitRates_ShouldTheExchangeRates()
    {
        // arrange
        ExchangeRateProviderRepository.UpsertExchangeRate(new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m)));

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRate").AppendPathSegment("defaultRates");
        var response = await _client.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRate>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEquivalentTo(new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m))
        });
    }

    [SetUp]
    public async Task SetUp()
    {
        ExchangeRateProviderRepository = new ExchangeRateProviderRepositoryInMemory();
        var hostBuilder = new TestApplicationHostBuilder(ExchangeRateProviderRepository);
        _host = hostBuilder.Configure().Build();
        await _host.StartAsync();
        _server = _host.GetTestServer();
        _server.BaseAddress = new Uri(ApiBaseAddress);
        _client = _server.CreateClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        _client?.Dispose();
        _server?.Dispose();
        await _host!.StopAsync();
        _host?.Dispose();
    }

}
