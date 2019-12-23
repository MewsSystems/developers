using Moq;
using System.Collections.Generic;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderFactoryTest
    {
        private readonly ExchangeRateProviderFactory _factory;

        public ExchangeRateProviderFactoryTest()
        {
            var providerCnb = Mock.Of<IExchangeRateProvider>(x => x.ProviderName == ProviderName.CNB);
            var providerEcb = Mock.Of<IExchangeRateProvider>(x => x.ProviderName == ProviderName.ECB);

            var availableProviders = new List<IExchangeRateProvider>() { providerCnb, providerEcb };

            _factory = new ExchangeRateProviderFactory(availableProviders);
        }

        public static IEnumerable<object[]> ValidProviderNames =>
            new List<object[]>
            {
                new object[] { ProviderName.CNB },
                new object[] { ProviderName.ECB }
            };

        [Theory]
        [MemberData(nameof(ValidProviderNames))]
        public void GetExchangeRateProvider_InputProviderName_ReturnsCorrectProvider(ProviderName providerName)
        {
            var result = _factory.GetExchangeRateProvider(providerName);

            Assert.True(result != null);
            Assert.True(result.ProviderName == providerName);
        }
    }
}
