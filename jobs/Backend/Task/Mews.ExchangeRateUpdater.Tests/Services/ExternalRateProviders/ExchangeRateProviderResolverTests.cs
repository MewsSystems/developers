using Mews.ExchangeRateUpdater.Services.ExternalRateProviders;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB;
using Mews.ExchangeRateUpdater.Services.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Mews.ExchangeRateUpdater.Tests.Services.ExternalRateProviders
{
    [TestFixture]
    public class ExchangeRateProviderResolverTests
    {
        [TestFixture]
        public class ExchangeRateRepositoryResolverTests
        {
            private Mock<IRestClient> _restClientMock;

            private Mock<IConfiguration> _configurationMock;

            private IEnumerable<IExchangeRateProvider> _exchangeRateProviders;

            private ExchangeRateProviderResolver _sut;

            [SetUp]
            public void SetUp()
            {
                _restClientMock = new Mock<IRestClient>();

                _configurationMock = new Mock<IConfiguration>();
                _configurationMock.Setup(x => x["ExchangeRateProvider"]).Returns("CNB");

                _exchangeRateProviders = new List<IExchangeRateProvider>
                {
                    new CNBExchangeRatesProvider(_restClientMock.Object, _configurationMock.Object)
                };

                _sut = new ExchangeRateProviderResolver(_exchangeRateProviders, _configurationMock.Object);
            }

            [Test]
            public void GetExchangeRateProvider_AtleastOnce_CallsCanProvideOnExchangeRatesProvider()
            {
                // Arrange
                var cnbExchangeRateProviderMock = new Mock<IExchangeRateProvider>();
                cnbExchangeRateProviderMock.Setup(x => x.CanProvide(It.IsAny<string>())).Returns(true);
                _exchangeRateProviders = new List<IExchangeRateProvider> { cnbExchangeRateProviderMock.Object };

                _sut = new ExchangeRateProviderResolver(_exchangeRateProviders, _configurationMock.Object);

                // Act
                _sut.GetExchangeRateProvider();

                // Assert
                cnbExchangeRateProviderMock.Verify(x => x.CanProvide(It.IsAny<string>()), Times.AtLeastOnce());
            }

            [Test]
            public void GetExchangeRateProvider_ForCNBProviderType_ReturnsInstanceOfCNBExchangeRatesProvider()
            {
                // Arrange
                IExchangeRateProvider expectedExchangeRateProvider = new CNBExchangeRatesProvider(_restClientMock.Object, _configurationMock.Object);

                // Act
                var actual = _sut.GetExchangeRateProvider();

                // Assert
                Assert.IsNotNull(actual);
                Assert.That(actual.GetType(), Is.EqualTo(expectedExchangeRateProvider.GetType()));
            }
        }
    }
}
