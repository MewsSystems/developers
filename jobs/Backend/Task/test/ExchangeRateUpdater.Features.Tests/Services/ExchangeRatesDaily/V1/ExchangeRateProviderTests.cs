using ExchangeRateUpdater.ApiClient.Client;
using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using ExchangeRateUpdater.Features.Exceptions;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using ExchangeRateUpdater.Models.Domain;
using FluentAssertions;
using Mews.Caching;
using Mews.Caching.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Features.Tests.Services.ExchangeRatesDaily.V1
{
    public class ExchangeRateProviderTests
    {
        private readonly Currency _sourceCurrency;
        private readonly Mock<ICnbClient> _mockcnbClient;
        private readonly Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private readonly Mock<ICustomCache> _mockCache;
        private readonly Mock<ICustomCacheFactory> _mockCustomCacheFactory;
        private readonly IExchangeRateProvider _sut;

        public ExchangeRateProviderTests()
        {
            _mockCache = new Mock<ICustomCache>();
            _mockcnbClient = new Mock<ICnbClient>();
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockCustomCacheFactory = new Mock<ICustomCacheFactory>();
            _mockCustomCacheFactory.Setup(x => x.GetOrCreate(It.IsAny<string>())).Returns(_mockCache.Object);

            _sut = new ExchangeRateProvider(
                _mockCustomCacheFactory.Object,
                _mockcnbClient.Object,
                _mockLogger.Object);
        }

        [Theory]
        [InlineData("EUR", "XYZ", new[] { "EUR", "JPY" })]
        public async Task When_CallCache_It_Return_ValidCurrencies(
            string validCurrency,
            string wrongCurrency,
             string[] currenciesReturned)
        {
            List<Currency> listcurrencies = new List<Currency>();
            foreach (var item in currenciesReturned)
                listcurrencies.Add(new Currency(item));

            SetupCustomCache(currenciesReturned);

            var actual = await _sut.GetExchangeRates(listcurrencies);

            actual.Should().NotBeNull();

            Assert.Contains(validCurrency, actual.Select(x => x.TargetCurrency.Code).ToList());
            Assert.DoesNotContain(wrongCurrency, actual.Select(x => x.TargetCurrency.Code).ToList());
        }


        [Fact]
        public async Task When_CallCache_It_Return_Null_Value()
        {
            List<Currency> listcurrencies = new List<Currency>();
            SetupCustomCacheError();

            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(async () => await _sut.GetExchangeRates(listcurrencies));
        }

        private void SetupCustomCache(IEnumerable<string> currenciesReturn)
        {
            _mockCache.Setup(x => x.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<ExchangeDailyCommand>>>()))
                .ReturnsAsync(GetExchangeDailyCommand(currenciesReturn));
        }

        private void SetupCustomCacheError()
        {

            _mockCache.Setup(x => x.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<ExchangeDailyCommand>>>()))
                .ReturnsAsync(Maybe<ExchangeDailyCommand>.Nothing);
        }

        public Maybe<ExchangeDailyCommand> GetExchangeDailyCommand(IEnumerable<string> currenciesReturn)
        {
            List<ExchangeRateDailyResponse> listExchangeRateResponse = new List<ExchangeRateDailyResponse>();

            foreach (var item in currenciesReturn)
            {
                listExchangeRateResponse.Add(new ExchangeRateDailyResponse()
                {
                    CurrencyCode = item
                });
            }

            Maybe<ExchangeDailyCommand> result = new ExchangeDailyCommand()
            {
                Payload = new ExchangeDailyResponse()
                {
                    Rates = listExchangeRateResponse
                }
            };
            return result;
        }
    }
}
