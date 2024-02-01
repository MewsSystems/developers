using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;

using System.Linq;
using ExchangeRateUpdater.services;
using ExchangeRateUpdater.responses;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using services;

namespace services.ExchangeRateUpdaterTests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    { 
          [Test]
        public async Task GetExchangeRates_Returns_ExchangeRates()
        {
            var exchangeRateProvider = new ExchangeRateProvider();

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("RUB")
            };

            var exchangeRates = await exchangeRateProvider.GetExchangeRates(currencies);

            Assert.IsNotNull(exchangeRates);
            Assert.AreEqual(3, exchangeRates.Count()); 


            var specificCurrency = exchangeRates.FirstOrDefault(rate =>
                rate.SourceCurrency.Code == "USD" );

            Assert.IsNotNull(specificCurrency, "Expected currency rate not found.");


        }

         [Test]
        public async Task GetRates_DailyRatesRequest_ReturnsValidResponse()
        {
            var result =  await CnbToolRequest.DailyRatesRequest(DateTime.UtcNow).Execute();
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOKCode);
            Assert.IsNotNull(result.OriginalResponse);
        }

         [Test]
        public async Task GetRates_OtherRatesRequest_ReturnsValidResponse()
        {
            var result =  await CnbToolRequest.OthersRatesRequest(DateTime.UtcNow).Execute();
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOKCode);
            Assert.IsNotNull(result.OriginalResponse);
            
        }


    }
}
