using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Infrastructure.UnitTests.Providers
{
    public class ExchangeRateProviderFactoryTests
    {
        [Fact]
        public void Create_ShouldReturnCorrectProvider()
        {
            // Arrange
            var targetCurrency = CurrencyCode.CZK;
            var providerSubstitute = Substitute.For<IExchangeRateProvider>();
            providerSubstitute.SupportedCurrencies.Returns(new[] { targetCurrency });

            var providers = new List<IExchangeRateProvider>
            {
                providerSubstitute
            };

            var subjectUnderTest = new ExchangeRateProviderFactory(providers, new NullLogger<ExchangeRateProviderFactory>());

            // Act
            var provider = subjectUnderTest.Create(targetCurrency);

            // Assert
            Assert.Contains(targetCurrency, provider.SupportedCurrencies);
        }
        
        [Fact]
        public void Create_ThrowsArgumentException_WhenNoSuitableProviderFound()
        {
            var targetCurrency = CurrencyCode.CZK;
            var subjectUnderTest = new ExchangeRateProviderFactory(new List<IExchangeRateProvider>(), new NullLogger<ExchangeRateProviderFactory>());

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => subjectUnderTest.Create(targetCurrency));
        }
    }
}
