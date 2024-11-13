using ExchangeRateUpdater.src;
using ExchangeRateUpdaterConsole.src;
using ExchangeRateUpdaterModels.Models;
using Moq;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task GetExchangeRatesAsync_FiltersCorrectly()
    {
        // Arrange
        var rates = new List<ExchangeRateModel>
        {
            new ExchangeRateModel(new CurrencyModel("USD"), new CurrencyModel("CZK"), 23.50m),
            new ExchangeRateModel(new CurrencyModel("EUR"), new CurrencyModel("CZK"), 25.00m),
            new ExchangeRateModel(new CurrencyModel("AUD"), new CurrencyModel("CZK"), 15.501m)
        };

        var mockApi = new Mock<ICzechNationalBankAPI>();

        mockApi.Setup(api => api.GetRatesAsync()).ReturnsAsync(rates);

        var provider = new ExchangeRateProvider();

        var currencies = new List<CurrencyModel>
        {
            new CurrencyModel("USD"),
            new CurrencyModel("EUR")
        };

        // Act
        IEnumerable<ExchangeRateModel> filteredRates = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Equal(2, filteredRates.Count());
        Assert.Contains(filteredRates, r => r.SourceCurrency.Code == "USD");
        Assert.Contains(filteredRates, r => r.SourceCurrency.Code == "EUR");
        Assert.DoesNotContain(filteredRates, r => r.SourceCurrency.Code == "AUD");
    }
}
