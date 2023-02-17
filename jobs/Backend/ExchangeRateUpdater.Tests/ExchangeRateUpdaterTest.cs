using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.ExchangeRateSources.CNB;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests
{
    public sealed class ExchangeRateUpdaterTest
    {
        private readonly IOptions<CNBSourceOptions> _options;
        private readonly IExchangeRateSource _exchangeRateSource;
        private readonly IExchangeRateProvider _rateProvider;

        public ExchangeRateUpdaterTest()
        {
            _options = Options.Create(new CNBSourceOptions()
            {
                Location = Data.SourceLocation.File,
                FileUri = "../../../Data/denni_kurz.txt"
            });
            _exchangeRateSource = new CNBExchangeRateSimpleSource(_options, NullLogger<CNBExchangeRateSource>.Instance);
            _rateProvider = new ExchangeRateProvider(_exchangeRateSource);
        }

        [Fact]
        public async void TestExchangeRateEqualProvider()
        {
            var currencies = new List<Currency>() { Constants.CZK };
            var ratesAsync = await _rateProvider.GetExchangeRatesAsync(currencies);
            var rates = _rateProvider.GetExchangeRates(currencies).OrderBy(c => c.Value);
            var orderedRates = ratesAsync.OrderBy(c => c.Value).ToList();
            Assert.Equal(rates, orderedRates);

        }

        [Fact]
        public void TestExchangeRateProvider()
        {
            var currencies = new List<Currency>() { Constants.EUR };
            var rates = _rateProvider.GetExchangeRates(currencies).ToList();
            Assert.Single(rates);
            Assert.Equal(Constants.EUR, rates.First().SourceCurrency);

        }
    }
}
