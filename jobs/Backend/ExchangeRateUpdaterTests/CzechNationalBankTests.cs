using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class CzechNationalBankTests
    {
        [Fact]
        public void GetAllExchangeRates_DownloadsDataAndParsesIt()
        {
            var config = A.Fake<ICzechNationalBankConfig>();
            var httpClientLineReader = A.Fake<IHttpClientLineReader>();

            var sourceExchangeRates = new CzechNationalBank(config,  httpClientLineReader);

            var exchangeRates = sourceExchangeRates.GetAllExchangeRates();

            A.CallTo(() => config.ExchangeRateUrl).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientLineReader.ReadLines(A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}
