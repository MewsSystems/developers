using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Tests.Unit.Helpers;

namespace ExchangeRateUpdater.Tests.Unit.Providers;

public class CzechBankExchangeRateProviderTests
{
    #region Private Fields
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
    private readonly CzechBankExchangeRateProvider _sut;
    #endregion

    #region Initialization
    public CzechBankExchangeRateProviderTests()
    {
        _exchangeRateServiceMock = new Mock<IExchangeRateService>(MockBehavior.Default);
        _sut = new CzechBankExchangeRateProvider(_exchangeRateServiceMock.Object);
    }
    #endregion

    [Theory]
    [MemberData(nameof(CzechBankExchangeRateProviderTestHelper.GetData), MemberType = typeof(CzechBankExchangeRateProviderTestHelper))]
    public async void GetExchangeRatesAsync_DifferentRequests_ReturnCorrespondingRates(List<Currency> targetCurrencies)
    {
        var currencies = GetCurrencies();
        var expectedResult = targetCurrencies?.Count(x => currencies.Select(y => y.Code).Contains(x.Code)) ?? 0;
        SetupDependencies(currencies);

        var exchangeRates = await _sut.GetExchangeRatesAsync(targetCurrencies);

        Assert.Equal(expectedResult, exchangeRates.Count);
    }

    #region Helper methods

    private void SetupDependencies(IReadOnlyCollection<Currency> currencies)
    {
         _exchangeRateServiceMock
            .Setup(x => x.GetCurrenciesAsync(It.IsAny<DateTime?>()))
            .ReturnsAsync(currencies);
    }

    private List<Currency> GetCurrencies()
    {
        return new List<Currency>
        {
            new("USA", "dollar", 1, "USD", 24.180M),
            new("EMU", "euro", 1, "EUR", 24.610M)
        };
    }

    #endregion
}