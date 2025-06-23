using API.Factory;
using API.Interfaces;
using API.Models;
using Moq;

namespace API.UnitTests
{
    public class ExchangeRateProviderFactoryTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Dictionary<string, Type> _providerTypeMap;
        private readonly ExchangeRateProviderFactory _factory;

        public ExchangeRateProviderFactoryTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _providerTypeMap = new Dictionary<string, Type>
            {
                { "CzechNationalBank", typeof(MockExchangeRateProvider) }
            };

            _factory = new ExchangeRateProviderFactory(_httpClientFactoryMock.Object, _providerTypeMap);
        }

        [Fact]
        public void GetExchangeRateProvider_ShouldReturnProvider_ForValidProvider()
        {
            // Arrange
            var mockHttpClient = new HttpClient();
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

            // Act
            var provider = _factory.GetExchangeRateProvider("CzechNationalBank");

            // Assert
            Assert.NotNull(provider);
            Assert.IsType<MockExchangeRateProvider>(provider);
        }

        [Fact]
        public void GetExchangeRateProvider_ShouldThrowArgumentException_ForInvalidProvider()
        {
            // Arrange
            var invalidProviderType = "INVALID";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _factory.GetExchangeRateProvider(invalidProviderType));
        }

        [Fact]
        public void GetExchangeRateProvider_ShouldThrowException_WhenServiceTypeCannotBeCreated()
        {
            // Arrange
            _providerTypeMap["CzechNationalBank"] = typeof(UncreatableExchangeRateProvider);
            var factoryWithUncreatableType = new ExchangeRateProviderFactory(_httpClientFactoryMock.Object, _providerTypeMap);

            // Act & Assert
            Assert.Throws<MissingMethodException>(() => factoryWithUncreatableType.GetExchangeRateProvider("CzechNationalBank"));
        }
    }

    // Mock implementation of IExchangeRateProvider for testing
    public class MockExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public MockExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    // Class with no constructor that accepts HttpClient
    public class UncreatableExchangeRateProvider : IExchangeRateProvider
    {
        private UncreatableExchangeRateProvider() { }

        public Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
