using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class CzechNationalBankExchangeRatesTests
    {
        [Fact]
        public void GetAllExchangeRates_DownloadsDataAndParsesIt()
        {
            var config = A.Fake<ICzechNationalBankConfig>();
            var httpClientLineReader = A.Fake<IHttpClientLineReader>();
            var parser = A.Fake<ICzechNationalBankExchangeRateParser>();

            var sourceExchangeRates = new CzechNationalBank(config, parser, httpClientLineReader);

            var exchangeRates = sourceExchangeRates.GetAllExchangeRates();

            A.CallTo(() => config.ExchangeRateUrl).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientLineReader.ReadLines(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => parser.ConvertToExchangeRates(A<IAsyncEnumerable<string>>._)).MustHaveHappenedOnceExactly();
        }
    }
}
