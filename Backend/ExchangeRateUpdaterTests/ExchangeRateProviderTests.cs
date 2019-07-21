using NUnit.Framework;
using ExchangeRateUpdater;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Tests
{
    public class ExchangeRateProviderTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ReturnsCorrectValue()
        {
            List<string> testCurrencySource = new List<string> {"19 Jul 2019 #138\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|16.055\nBrazil|real|1|BRL|6.093\nBulgaria|lev|1|BGN|13.061\nCanada|dollar|1|CAD|17.431\nChina|renminbi|1|CNY|3.309\nCroatia|kuna|1|HRK|3.457\nDenmark|krone|1|DKK|3.421\nEMU|euro|1|EUR|25.545\nHongkong|dollar|1|HKD|2.916\nHungary|forint|100|HUF|7.851\nIceland|krona|100|ISK|18.207\nIMF|SDR|1|XDR|31.436\nIndia|rupee|100|INR|33.066\nIndonesia|rupiah|1000|IDR|1.633\nIsrael|shekel|1|ILS|6.434\nJapan|yen|100|JPY|21.119\nMalaysia|ringgit|1|MYR|5.532\nMexico|peso|1|MXN|1.196\nNew Zealand|dollar|1|NZD|15.407\nNorway|krone|1|NOK|2.657\nPhilippines|peso|100|PHP|44.603\nPoland|zloty|1|PLN|6.001\nRomania|new leu|1|RON|5.401\nRussia|rouble|100|RUB|36.108\nSingapore|dollar|1|SGD|16.742\nSouth Africa|rand|1|ZAR|1.633\nSouth Korea|won|100|KRW|1.937\nSweden|krona|1|SEK|2.428\nSwitzerland|franc|1|CHF|23.156\nThailand|baht|100|THB|73.884\nTurkey|lira|1|TRY|4.033\nUnited Kingdom|pound|1|GBP|28.499\nUSA|dollar|1|USD|22.756\n"};

            var inputCurrencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("XYZ")
            };

            string[] expectedExchangeRatesToStringValues = {
                "USD/CZK=22.756",
                "EUR/CZK=25.545",
                "JPY/CZK=0.21119"
            };

            var rateSourceProviderMock = new Mock<IRateSourceProvider>();
            var rateSourceParcer = new RateSourceParcer();
            rateSourceProviderMock.Setup(x => x.GetRateSourcesByUrl(It.IsAny<IEnumerable<string>>())).Returns(testCurrencySource);

            var exchangeRateProvider = new ExchangeRateProvider(rateSourceParcer, rateSourceProviderMock.Object);
            var result = exchangeRateProvider.GetExchangeRates(inputCurrencies).ToList<ExchangeRate>();

            Assert.IsNotNull(result, "GetExchangeRates() returned null");
            Assert.AreEqual(result.Count(), expectedExchangeRatesToStringValues.Length, "Returned exchange rates count is not equal to expected."); ;
            
            for (int i = 0; i < expectedExchangeRatesToStringValues.Length; i++)
            {
                Assert.AreEqual(result.ElementAt(i).ToString(), expectedExchangeRatesToStringValues[i]);
            }
            
            Assert.Pass();
        }
    }
}