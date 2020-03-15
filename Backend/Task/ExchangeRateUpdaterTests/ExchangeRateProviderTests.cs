using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IExchangeRateClient> _exchangeRateClient = new Mock<IExchangeRateClient>();
        
        [SetUp]
        public void Setup()
        {
        }

        [Test, TestCaseSource(typeof(TestDataSource))]
        public async Task<IEnumerable<ExchangeRate>> ExchangeRateProvider_TestData_Ok(IEnumerable<ExchangeRate> source, IEnumerable<Currency> input)
        {
            _exchangeRateClient.Setup(x=> x.GetExchangeRates()).ReturnsAsync(source);
            var target = new ExchangeRateProvider(_exchangeRateClient.Object);
            return await target.GetExchangeRates(input);
        }

        private class TestDataSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new TestCaseData(CompleteList, Currencies).Returns(CompleteList).SetName(nameof(CompleteList));
                yield return new TestCaseData(EuroTargetList, Currencies).Returns(EuroTargetList).SetName(nameof(EuroTargetList));
                yield return new TestCaseData(NoMatchList, Currencies).Returns(new ExchangeRate[]{}).SetName(nameof(NoMatchList));
                yield return new TestCaseData(PartiallyMatchList, Currencies).Returns(PartiallyMatchList.Where(x => x.SourceCurrency.Code == "USD").ToArray()).SetName(nameof(PartiallyMatchList));
                yield return new TestCaseData(NullValuesList, Currencies).Returns(NullValuesList.Where(x => x?.SourceCurrency?.Code == "USD").ToArray()).SetName(nameof(NullValuesList));
            }
        }
        
        private static readonly Currency[] Currencies = {new Currency("USD"), new Currency("CZK"), new Currency("EUR"), null, new Currency(null)  };
        
        private static readonly IEnumerable<ExchangeRate> CompleteList = new[]
        {
            new ExchangeRate(new Currency("USD"),new Currency("CZK"), 2.0m ),
            new ExchangeRate(new Currency("CZK"),new Currency("USD"), 0.5m ),
            new ExchangeRate(new Currency("EUR"),new Currency("USD"), 2.0m ),
            new ExchangeRate(new Currency("USD"),new Currency("EUR"), 0.5m ),
            new ExchangeRate(new Currency("EUR"),new Currency("CZK"), 2.0m ),
            new ExchangeRate(new Currency("CZK"),new Currency("EUR"), 0.5m ),
        };

        private static readonly IEnumerable<ExchangeRate> EuroTargetList = new[]
        {
            new ExchangeRate(new Currency("USD"),new Currency("EUR"), 0.5m ),
            new ExchangeRate(new Currency("CZK"),new Currency("EUR"), 0.5m ),
        };

        private static readonly IEnumerable<ExchangeRate> NoMatchList = new[]
        {
            new ExchangeRate(new Currency("RUB"),new Currency("BYR"), 0.5m ),
        };

        private static readonly IEnumerable<ExchangeRate> PartiallyMatchList = new[]
        {
            new ExchangeRate(new Currency("USD"),new Currency("EUR"), 0.5m ),
            new ExchangeRate(new Currency("RUB"),new Currency("BYR"), 0.5m )
        };
        
        private static readonly IEnumerable<ExchangeRate> NullValuesList = new[]
        {
            new ExchangeRate(new Currency("USD"),new Currency("EUR"), 0.5m ),
            null,
            new ExchangeRate(null,new Currency("EUR"), 0.5m ),
            new ExchangeRate(new Currency("EUR"), null, 0.5m ),
            new ExchangeRate(new Currency(null), new Currency( null), 0.5m )
        };
    }
}