using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Infrastructure.Http;
using ExchangeRateUpdater.Core.Interfaces;

namespace ExchangeRateUpdater.Tests.Unit
{
    public class CnbExchangeRateRepositoryTest
    {
        private readonly Mock<ICnbApiClient> _mockApiClient;
        private readonly Mock<ILogger<CnbExchangeRateRepository>> _mockLogger;
        private readonly CnbExchangeRateRepository _repository;

        public CnbExchangeRateRepositoryTest()
        {
            _mockApiClient = new Mock<ICnbApiClient>();
            _mockLogger = new Mock<ILogger<CnbExchangeRateRepository>>();
            _repository = new CnbExchangeRateRepository(_mockApiClient.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithValidData_ReturnsCorrectlyParsedRates()
        {
            var fakeApiResponse = @"21 Jun 2024 #118
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.426
Brazil|real|1|BRL|4.274
Canada|dollar|1|CAD|17.009";


            _mockApiClient
                .Setup(client => client.GetLatestExchangeRatesAsync())
                .ReturnsAsync(fakeApiResponse);

            var result = await _repository.GetExchangeRatesAsync();
            var rates = result.ToList();

            Assert.NotNull(result);
            Assert.Equal(3, rates.Count);

            var audRate = rates.FirstOrDefault(r => r.SourceCurrency.Code == "AUD");
            Assert.NotNull(audRate);
            Assert.Equal("AUD", audRate.SourceCurrency.Code);
            Assert.Equal("CZK", audRate.TargetCurrency.Code);
            Assert.Equal(15.426m, audRate.Value);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithEmptyString_ThrowsHttpRequestException()
        {

            var fakeApiResponse = string.Empty;

            _mockApiClient
                .Setup(client => client.GetLatestExchangeRatesAsync())
                .ReturnsAsync(fakeApiResponse);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _repository.GetExchangeRatesAsync());

            Assert.IsType<ArgumentException>(exception.InnerException);
        }
    }
}