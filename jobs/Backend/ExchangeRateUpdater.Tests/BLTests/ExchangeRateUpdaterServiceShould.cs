using ExchangeRateUpdater.BL.Implementations;
using ExchangeRateUpdater.BL.Models;
using ExchangeRateUpdater.DAL.Implementations;
using ExchangeRateUpdater.Tests.Utility;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.BLTests
{
    public class ExchangeRateUpdaterServiceShould
    {
        private readonly ExchangeRateUpdaterService _exchangeRateUpdaterService;
        private readonly string _CNBUrl; 
        public ExchangeRateUpdaterServiceShould()
        {
            _CNBUrl = TestParameters.GetCNBWebsite();
            var _loggerDataScrapper = new Mock<ILogger<DataScrapper>>();
            var _dataScrapper = new DataScrapper(_loggerDataScrapper.Object); 
            var _loggerExchangeRateUpdaterService = new Mock<ILogger<ExchangeRateUpdaterService>>();
            _exchangeRateUpdaterService = new ExchangeRateUpdaterService(_dataScrapper, _loggerExchangeRateUpdaterService.Object);
        }
        [Fact]
        public void should_return_mapped_rates_success()
        {
            var _currencies = TestParameters.GetSampleCurrencies();
            var result = _exchangeRateUpdaterService.GetExchangeRateMappedFromSource(_currencies, _CNBUrl);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ExchangeRate>>(result);
        }
        [Fact]
        public void should_return_emptyIEnumerable_if_no_currency_in_input_()
        {
            var _currencies = new List<Currency>().AsEnumerable();
            var result = _exchangeRateUpdaterService.GetExchangeRateMappedFromSource(_currencies, _CNBUrl);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public void should_not_return_rate_for_default_currency()
        {
            var _currencies = TestParameters.GetDefaultCurrencyOnly();
            var result = _exchangeRateUpdaterService.GetExchangeRateMappedFromSource(_currencies, _CNBUrl);

            Assert.NotNull(result);
            Assert.Empty(result);
        }


    }
}
