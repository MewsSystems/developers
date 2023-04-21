using Castle.Core.Logging;
using ExchangeRateUpdater.BusinessLogic.Cnb.Services.Implementations;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants;
using ExchangeRateUpdater.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests.Cnb
{
    public class CnbExchangeServiceTests
    {
        IConfiguration _configuration;
        public CnbExchangeServiceTests() {
            _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection(CnbConstants.SettingsSectionKey);
        }

        [Fact]
        public void WhenGetExchangeRates_WhitNullCurrencies_ShouldReturnEmpty()
        {
            var mockedExchangeClient = new Mock<IExchangeClient>();
            var mockedLogger = new Mock<ILogger<CnbExchangeService>>();

            var result = new CnbExchangeService(_configuration, mockedExchangeClient.Object, mockedLogger.Object).GetExchangeRates(null);

            Assert.Empty(result);
        }

        [Fact]
        public void WhenGetExchangeRates_WhitExchangeRate_ShouldReturnExchangeRate()
        {
            var testCurrencyCode = "EUR";
            var expectedResult = $"{testCurrencyCode}/{CnbConstants.DefaultCurrencyCode}=23,550";
            var testResult = "21 Apr 2023 #78\r\nCountry|Currency|Amount|Code|Rate\r\nAustralia|dollar|1|AUD|14.366\r\nBrazil|real|1|BRL|4.248\r\nBulgaria|lev|1|BGN|12.041\r\nCanada|dollar|1|CAD|15.852\r\nChina|renminbi|1|CNY|3.114\r\nDenmark|krone|1|DKK|3.160\r\nEMU|euro|1|EUR|23.550";
            var mockedExchangeClient = new Mock<IExchangeClient>();
            mockedExchangeClient.Setup(x => x.GetExchangeRateTxtAsync(testCurrencyCode)).Returns(Task.FromResult(testResult));
            var mockedLogger = new Mock<ILogger<CnbExchangeService>>();
            var result = new CnbExchangeService(_configuration, mockedExchangeClient.Object, mockedLogger.Object).GetExchangeRates(new List<Currency>() { new Currency(testCurrencyCode) });

            Assert.Equal(expectedResult, result.First().ToString());
        }

        [Fact]
        public void WhenGetExchangeRates_WhitFxExchangeRate_ShouldReturnExchangeRate()
        {
            var testCurrencyCode = "AUD";
            var expectedResult = $"{testCurrencyCode}/{CnbConstants.DefaultCurrencyCode}=0,04294"; //100 units and 4.294 value
            var testResult = "Country|Angola\r\nCurrency|AOA\r\nDate|Amount|Rate\r\n31 Jan 2023|100|4.375\r\n28 Feb 2023|100|4.395\r\n31 Mar 2023|100|4.294";
            var mockedExchangeClient = new Mock<IExchangeClient>();
            mockedExchangeClient.Setup(x => x.GetExchangeRateTxtAsync(testCurrencyCode)).Returns(Task.FromResult(string.Empty));
            mockedExchangeClient.Setup(x => x.GetFxExchangeRateTxtAsync(testCurrencyCode)).Returns(Task.FromResult(testResult));
            var mockedLogger = new Mock<ILogger<CnbExchangeService>>();

            var result = new CnbExchangeService(_configuration, mockedExchangeClient.Object, mockedLogger.Object).GetExchangeRates(new List<Currency>() { new Currency(testCurrencyCode) });

            Assert.Equal(expectedResult, result.First().ToString());
        }
    }
}