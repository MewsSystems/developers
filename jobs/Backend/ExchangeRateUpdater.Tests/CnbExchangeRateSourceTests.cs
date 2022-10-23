namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateSourceTests
    {
        private CnbExchangeRateSource _exchangeRateSource;

        [SetUp]
        public void Setup()
        {
            var options = new CnbExchangeRateSource.Options
            {
                Url = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
            };

            _exchangeRateSource = new CnbExchangeRateSource(options);
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