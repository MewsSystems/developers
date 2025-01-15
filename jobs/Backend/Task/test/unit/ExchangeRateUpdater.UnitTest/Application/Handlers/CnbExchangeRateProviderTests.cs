using ExchangeRateUpdater.Application.Contracts.Caching;
using ExchangeRateUpdater.Application.Contracts.Persistence;
using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates;
using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies;
using ExchangeRateUpdater.Domain.Const;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.UnitTest.Application.Handlers
{
    public class CnbExchangeRateProviderTests
    {
        private readonly CnbExchangeRateProvider _cnbProvider;
        private readonly Mock<ICnbExchangeRateRepository> _cnbExchangeRateRepository;
        private readonly Mock<ICacheService> _cache;
        private readonly GetExchangeRatesQuery _request;
        private readonly ICollection<ExchangeRate> _response;

        public CnbExchangeRateProviderTests()
        {
            _cnbExchangeRateRepository = new Mock<ICnbExchangeRateRepository>();
            _cache = new Mock<ICacheService>();
            _request = new GetExchangeRatesQuery() { Currencies = ["AUD"] };
            _response =
            [
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 14M)
            ];

            _cache
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(false);

            _cache
                .Setup(x => x.Add(It.IsAny<string>(), It.IsAny<IEnumerable<ExchangeRate>>()));

            _cache
                .Setup(x => x.Get<IEnumerable<ExchangeRate>>(It.IsAny<string>()))
                .Returns(_response);

            _cnbExchangeRateRepository
                .Setup(x => x.GetExchangeRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_response);

            _cnbProvider = new CnbExchangeRateProvider(_cnbExchangeRateRepository.Object, _cache.Object);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_Ok()
        {
            // Act
            var exChangeResponse = await _cnbProvider.GetExchangeRatesAsync(_request, CancellationToken.None);

            // Assert
            exChangeResponse.Should().NotBeNull();
            exChangeResponse.First().Value.Should().Be(_response.First().Value);

            _cache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once, "_cache Exist not called");
            _cache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<IEnumerable<ExchangeRate>>()), Times.Once, "_cache Add not called");
            _cache.Verify(x => x.Get<IEnumerable<ExchangeRate>>(It.IsAny<string>()), Times.Once, "_cache Get not called");
            _cnbExchangeRateRepository.Verify(x => x.GetExchangeRatesAsync(It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()), Times.Once, "GetExchangeRatesAsync not called");
        }

        [InlineData("FAKE", false)]
        [InlineData(ProviderConstants.CnbProviderCode, true)]
        [Theory]
        public void CandHandle_Ok(string providerCode, bool isValid)
        {
            // Act
            var canHandle = _cnbProvider.CanHandle(providerCode);

            // Assert
            canHandle.Should().Be(isValid);
        }
    }
}
