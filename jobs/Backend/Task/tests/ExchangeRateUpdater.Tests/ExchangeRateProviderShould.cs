using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderShould : IDisposable
{
    private readonly CnbClient _cnbClient;
    private readonly HttpClient _httpClient;

    public ExchangeRateProviderShould()
    {
        _httpClient = new HttpClient();
        _cnbClient = new CnbClient(_httpClient, NullLogger<CnbClient>.Instance);
    }
    
    [Fact]
    public void ReturnExchangeRates()
    {
        // act
        var provider = new ExchangeRateProvider(_cnbClient);
        var rates = provider.GetExchangeRates(new[] { new Currency("EUR") });

        // assert
        var rate = Assert.Single(rates);
        Assert.Equal("EUR", rate.SourceCurrency.Code);
        Assert.Equal("CZK", rate.TargetCurrency.Code);
        
        // let's expect that the exchange rate is positive (⊙_⊙;)
        Assert.True(rate.Value > 0);
    }
    
    [Fact]
    public void NotReturnUnknownCurrency()
    {
        // act
        var provider = new ExchangeRateProvider(_cnbClient);
        var rates = provider.GetExchangeRates(new[] { new Currency("SPL") });

        // assert
        // SPL – Seborga Luigino (Principality of Seborga) is not expected to be supported by the CNB
        Assert.Empty(rates);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}