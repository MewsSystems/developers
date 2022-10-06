using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Providers.ProvidersStrategies;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderStrategyFactoryTests : MoqMeUp<ExchangeRateProviderStrategyFactory>
    {
        [Fact]
        public void GetStrategy_DefaultBehaviour_ReturnsCorrectStrategy()
        {
            // Arrange
            this.Get<IHttpClientFactory>();

            // Act
            var target = this.Build();
            var act = target.GetStrategy(ExchangeRateProviderCountry.CzechRepublic);

            // Assert
            act.Should().BeOfType<CzechNationalBankExchangeRateProvider>();
        }
    }
}