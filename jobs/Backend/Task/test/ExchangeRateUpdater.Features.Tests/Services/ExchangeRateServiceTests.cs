using ExchangeRateUpdater.Features.Exceptions;
using ExchangeRateUpdater.Features.Services;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using ExchangeRateUpdater.Models.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Features.Tests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly IExchangeRateService _sut;
        private readonly Mock<IExchangeRateProvider> _mockExchangeRateProvider;
        private readonly Mock<ILogger<ExchangeRateService>> _mockLogger;

        public ExchangeRateServiceTests()
        {
            _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();
            _mockLogger = new Mock<ILogger<ExchangeRateService>>();

            _sut = new ExchangeRateService(_mockExchangeRateProvider.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task When_CurrencyInputList_IsNull()
        {
            IEnumerable<CurrencyModel> nullInput = null;
            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(async () => await _sut.GetExchangeRates(nullInput));

        }

        [Fact]
        public async Task When_CurrencyInputList_IsEmpty()
        {
            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(async () => await _sut.GetExchangeRates(Enumerable.Empty<CurrencyModel>()));

        }

        [Theory]
        [InlineData("EUR", "XYZ", new[] { "EUR", "JPY" })]
        public async Task When_GetExchangeRates_Obtain_ExchangeRateModel_Correctly(
            string validCurrency,
            string wrongCurrency,
            string[] currencies)
        {
            List<CurrencyModel> expected = new List<CurrencyModel>();

            List<ExchangeRate> result = new List<ExchangeRate>();
            foreach (var item in currencies)
            {
                expected.Add(new CurrencyModel(item));
                result.Add(new ExchangeRate(new Currency("CZK"), new Currency(item), 1));
            }

            SetupExchangeRateProvider(result);
            var actual = await _sut.GetExchangeRates(expected);

            actual.Should().NotBeNull();
            Assert.Equal(2, actual.Count());

            Assert.Contains(validCurrency, actual.Select(x => x.TargetCurrency.Code).ToList());
            Assert.DoesNotContain(wrongCurrency, actual.Select(x => x.TargetCurrency.Code).ToList());

        }

        private void SetupExchangeRateProvider(IEnumerable<ExchangeRate> exchangeRates)
        {
            _mockExchangeRateProvider.Setup(x => x.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()))
                .ReturnsAsync(exchangeRates);
        }

    }
}
