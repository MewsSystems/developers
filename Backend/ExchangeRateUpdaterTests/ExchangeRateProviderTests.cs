using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{
    [TestClass()]
    public class ExchangeRateProviderTests
    {
        private Mock<IFileDownloadService> GetMockedDownloadService(string returnVal)
        {
            var mockDownloadService = new Mock<IFileDownloadService>();
            mockDownloadService.Setup(service => service
                .DownloadFileContent(It.IsAny<string>()))
                .Returns((string url) => returnVal);
            return mockDownloadService;
        }

        [DataTestMethod()]
        [DataRow("28.03.2021\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|16,873")]
        [DataRow("28.03.2021\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|XY")]
        [DataRow("země|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|16,873")]
        [DataRow("28.03.2021 #60\nměna|země|množství|kód|kurz\ndolar|Austrálie|1|AUD|16,873")]
        [DataRow("28.03.2021 #60\nzemě|měna|množství|kód|kurz")]
        public void GetExchangeRates_WhenInvalidFile_ThenThrowsException_Test(string exchangeRateFileContent)
        {
            Mock<IFileDownloadService> mockDownloadService = GetMockedDownloadService(exchangeRateFileContent);
            var provider = new ExchangeRateProvider(mockDownloadService.Object);

            Assert.ThrowsException<Exception>(() => provider.GetExchangeRates(new Currency[] { new Currency("AUD") }));
        }

        [TestMethod()]
        [DataRow("28.03.2021 #60\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|16,873|\nFilipíny|peso|100|PHP|45,656\nJižní Afrika|rand|1|ZAR|1,475")]
        public void GetExchangeRates_WhenValidFile_ThenReturnAllDesiredCurrencies_Test(string exchangeRateFileContent)
        {
            Mock<IFileDownloadService> mockDownloadService = GetMockedDownloadService(exchangeRateFileContent);
            var provider = new ExchangeRateProvider(mockDownloadService.Object);
            var expectedSourceCurr = new Currency("CZK");
            var expectedTargetCurr = new Currency("PHP");
            var expectedExchangeRate = new ExchangeRate(expectedSourceCurr, expectedTargetCurr, (decimal)45.656/100);

            var expectedSourceCurr2 = new Currency("CZK");
            var expectedTargetCurr2 = new Currency("ZAR");
            var expectedExchangeRate2 = new ExchangeRate(expectedSourceCurr2, expectedTargetCurr2, (decimal)1.475);


            var returnedRates = provider.GetExchangeRates(new List<Currency>() { expectedTargetCurr, expectedTargetCurr2 });

            Assert.AreEqual(2, returnedRates.Count());
            Assert.IsTrue(returnedRates.Contains(expectedExchangeRate));
            Assert.IsTrue(returnedRates.Contains(expectedExchangeRate2));
        }
    }
}