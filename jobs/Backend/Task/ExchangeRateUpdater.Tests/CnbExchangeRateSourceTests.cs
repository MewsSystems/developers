using ExchangeRateUpdater.Cnb;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateSourceTests
    {
        private CnbClient _exchangeRateSource;

        [SetUp]
        public void Setup()
        {
            var options = new CnbClient.Options
            {
                Url = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
            };

            _exchangeRateSource = new CnbClient(options);
        }

        [Test]
        public async Task GetRates()
        {
            var rateInfo = await _exchangeRateSource.GetLatestExchangeRatesAsync();
            Console.WriteLine(rateInfo.Date);

            foreach (var rate in rateInfo.Rates)
            {
                Console.WriteLine(rate);
            }
        }
    }
}