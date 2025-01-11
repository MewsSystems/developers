using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Services;

namespace TestExchangeRateUpdater.Tests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    private ExchangeRateService _exchangeRateService;

    [SetUp]
    public void Setup()
    {
        _exchangeRateService = new ExchangeRateService();
    }

    [Test]
    public void GetExchangeRates_ShouldReturn_CorrectExchangeRatesDTO()
    {
        // Arrange
        var expected = new ExchangeRatesDTO
        {
            Rates = new List<ExchangeRateDTO>
            {
                new ()
                {
                    ValidFor = "2025-01-11",
                    Order = 1,
                    Currency = "Currency",
                    Country = "Country",
                    Amount = 1,
                    CurrencyCode = "CUR",
                    Rate = 10.00M
                }
            }
        };

        // Act
        var actual = _exchangeRateService.GetExchangeRates();

        // Assert
        Assert.That(actual.Rates, Is.EqualTo(expected.Rates));
    }
}
