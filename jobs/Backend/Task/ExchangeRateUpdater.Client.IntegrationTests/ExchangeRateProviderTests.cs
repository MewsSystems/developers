using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Client.Bootstrap;
using ExchangeRateUpdater.Client.Client;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExchangeRateUpdater.Client.IntegrationTests;

public class ExchangeRateProviderTests
{
    private readonly ProviderClient _sut;
    
    public ExchangeRateProviderTests()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddCzechExchangeRateProvider();
        var serviceProvider = services.BuildServiceProvider();

        _sut = new ProviderClient(serviceProvider.GetRequiredService<HttpClient>());
    }
    
    [Fact]
    public async Task GetAsync_Returns_LatestExchangeRates_WhenNoParametersSpecified()
    {
        // arrange
        
        // act
        var actual = await _sut.GetAsync();

        // assert
        Assert.NotEmpty(actual);
        Assert.Equal(31, actual.Count());
    }
    
    [Fact]
    public async Task GetAsync_Returns_LatestExchangeRates_WhenDateSpecified()
    {
        // arrange
        
        // act
        var actual = await _sut.GetAsync(new DateTime(2023, 5, 12));

        // assert
        Assert.NotEmpty(actual);
        
        Assert.Contains(actual, pair =>
            pair.Country == "EMU" &&
            pair.Currency == "euro" &&
            pair.Amount == 1m &&
            pair.Code == "EUR" &&
            pair.Rate == 23.605m);
    }
}