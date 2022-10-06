using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Providers.ProvidersStrategies;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderStrategyFactoryTests : MockBase<ExchangeRateProviderStrategyFactory>
    {
        [Fact]
        public void GetStrategy_DefaultBehaviour_ReturnsCorrectStrategy()
        {
            // Arrange
            Mocker.MockOf<IHttpClientFactory>();

            // Act
            var act = this.Subject.GetStrategy(ExchangeRateProviderCountry.CzechRepublic);

            // Assert
            act.Should().BeOfType<CzechNationalBankExchangeRateProvider>();
        }
    }
}