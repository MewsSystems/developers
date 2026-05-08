using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates;
using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies;
using ExchangeRateUpdater.Domain.Const;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace ExchangeRateUpdater.UnitTest.Application.Handlers
{
    public class GetExchangeRatesQueryHandlerTests
    {
        private readonly ICollection<IExchangeRateProviderStrategy> _providerStrategies = [];
        private readonly GetExchangeRatesQuery _request;
        private readonly GetExchangeRatesQueryHandler _handler;
        private readonly ICollection<GetExchangeRatesQueryResponse> _response;

        public GetExchangeRatesQueryHandlerTests()
        {
            _request = new GetExchangeRatesQuery() { Currencies = ["AUD"] };
            _handler = new GetExchangeRatesQueryHandler(_providerStrategies);

            _response =
            [
                new GetExchangeRatesQueryResponse()
            {
                SourceCurrency = "AUD",
                TargetCurrency = "CZK",
                Value = 15M,
                ValidFor = DateTime.Today
            }];
        }

        [Fact]
        public async Task GetExchangeRates_Handler_WorksAsync()
        {
            // Arrange
            var providerStrategy = new Mock<IExchangeRateProviderStrategy>();

            providerStrategy
                .Setup(x => x.CanHandle(ProviderConstants.CnbProviderCode))
                .Returns(true);

            providerStrategy
                .Setup(x => x.GetExchangeRatesAsync(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_response);

            _providerStrategies.Add(providerStrategy.Object);

            // Act
            var response = await _handler.Handle(_request, CancellationToken.None);

            // Assert
            providerStrategy.Verify(
                x => x.CanHandle(ProviderConstants.CnbProviderCode),
                Times.Once);

            providerStrategy.Verify(
                x => x.GetExchangeRatesAsync(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()),
                Times.Once);

            response.Should().NotBeNull();
        }

        [Fact]
        public void GetExchangeRates_Throws_If_There_Are_Not_Matching_ProviderStrategy()
        {
            // Arrange
            var providerStrategy = new Mock<IExchangeRateProviderStrategy>();

            providerStrategy
                .Setup(x => x.CanHandle(ProviderConstants.CnbProviderCode))
                .Returns(false);

            _providerStrategies.Add(providerStrategy.Object);

            // Act
            var act = async () => await _handler.Handle(_request, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ValidationException>();
        }
    }
}
