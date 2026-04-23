using ExchangeRateUpdater.Options;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Options;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private IOptions<ExchangeRateProviderOptions> GetMockOptions()
    {
        var mockOptions = new Mock<IOptions<ExchangeRateProviderOptions>>();
        mockOptions.Setup(o => o.Value).Returns(new ExchangeRateProviderOptions
        {
            BaseUrl = "https://daily-rates.com",
            OtherCurrenciesUrl = "https://monthly-rates.com"
        });
        
        return mockOptions.Object;
    }

    [Fact]
    public async Task GetExchangeRates_ShouldReturnRatesFromMockedDailyAndMonthlySources()
    {
        var dailyRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.762m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.145m)
        };

        var monthlyRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 0.15662m),
            new ExchangeRate(new Currency("THB"), new Currency("CZK"), 0.6655m)
        };

        var mockProvider = new Mock<ExchangeRateProvider>(GetMockOptions()) { CallBase = true };
        
        mockProvider.Protected()
            .Setup<Task<IEnumerable<ExchangeRate>>>("GetDailyExchanges")
            .ReturnsAsync(dailyRates);
        
        mockProvider.Protected()
            .Setup<Task<IEnumerable<ExchangeRate>>>("GetMonthlyExchanges")
            .ReturnsAsync(monthlyRates);

        var provider = mockProvider.Object;
        
        var result = await provider.GetExchangeRates(new List<Currency>
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("JPY"),
            new Currency("THB")
        });
        
        result = result.ToList();
        result.Should().HaveCount(4);
        result.Should().Contain(rate => rate.SourceCurrency.Code == "USD" && rate.Value == 22.762m);
        result.Should().Contain(rate => rate.SourceCurrency.Code == "EUR" && rate.Value == 25.145m);
        result.Should().Contain(rate => rate.SourceCurrency.Code == "JPY" && rate.Value == 0.15662m);
        result.Should().Contain(rate => rate.SourceCurrency.Code == "THB" && rate.Value == 0.6655m);
    }
    
    [Fact]
    public async Task GetExchangeRates_ShouldReturnEmptyWhenNoCurrenciesProvided()
    {
        var provider = new ExchangeRateProvider(GetMockOptions());
        
        var result = await provider.GetExchangeRates(new List<Currency>());
        
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRates_ShouldNotIncludeCalculatedRates()
    {
        var dailyRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.762m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.145m)
        };

        var monthlyRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 0.15662m),
            new ExchangeRate(new Currency("THB"), new Currency("CZK"), 0.6655m)
        };

        var mockProvider = new Mock<ExchangeRateProvider>(GetMockOptions()) { CallBase = true };
        
        mockProvider.Protected()
            .Setup<Task<IEnumerable<ExchangeRate>>>("GetDailyExchanges")
            .ReturnsAsync(dailyRates);
        
        mockProvider.Protected()
            .Setup<Task<IEnumerable<ExchangeRate>>>("GetMonthlyExchanges")
            .ReturnsAsync(monthlyRates);
        
        var provider = mockProvider.Object;
        
        var result = await provider.GetExchangeRates(new List<Currency>
        {
            new Currency("USD"),
            new Currency("JPY"),
            new Currency("CZK")
        });
        
        result = result.ToList();
        
        result.Should().HaveCount(2);
        result.Should().Contain(rate => rate.SourceCurrency.Code == "USD" && rate.Value == 22.762m);
        result.Should().Contain(rate => rate.SourceCurrency.Code == "JPY" && rate.Value == 0.15662m);
    }
}
