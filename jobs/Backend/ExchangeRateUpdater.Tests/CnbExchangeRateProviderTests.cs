using System.Net;
using ExchangeRateUpdater.CsvParser;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class CnbExchangeRateProviderTests
{
    [Test]
    public void CnbExchangeRateProviderTests_happyPath_provideExchangeRate()
    {
        var loggerMock = new Mock<ILogger<CnbExchangeRateProvider>>();
        var httpHandlerMock = new Mock<IHttpHandler>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var cnbCsvReaderMock = new Mock<ICnbCsvReader>();
        var endpoint = new ExchangeRateEndpoint("testEndpoint", "url", null);
        var currencyToSearch = new Currency("USD");

        httpHandlerMock.Setup(s => s.Get(It.IsAny<string>()))
            .Returns(new Func<object, HttpResponseMessage>(r => new HttpResponseMessage(HttpStatusCode.OK)));
        cnbCsvReaderMock.Setup(s => s.GetRecords(It.IsAny<Stream>()))
            .Returns(() => new []{ new CnbExchangeRateModel() { Code = new Currency("USD"), Rate = 5, Amount = 2}});

        var provider = new CnbExchangeRateProvider(loggerMock.Object, httpHandlerMock.Object, dateTimeProviderMock.Object, cnbCsvReaderMock.Object);
        provider.SetEndpoints(new []{ endpoint });
        provider.FetchRates();
        var exchangeRates = provider.GetExchangeRates(new[] { currencyToSearch }).ToList();

        Assert.AreEqual(1, exchangeRates.Count);
        Assert.AreEqual(2.5m, exchangeRates[0].Value);
        Assert.AreEqual("USD/CZK=2,5", exchangeRates[0].ToString());
    }

    [Test]
    public void CnbExchangeRateProviderTests_nonExistingCurrency_emptyResult()
    {
        var loggerMock = new Mock<ILogger<CnbExchangeRateProvider>>();
        var httpHandlerMock = new Mock<IHttpHandler>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var cnbCsvReaderMock = new Mock<ICnbCsvReader>();
        var endpoint = new ExchangeRateEndpoint("testEndpoint", "url", null);
        var currencyToSearch = new Currency("ABC");

        httpHandlerMock.Setup(s => s.Get(It.IsAny<string>()))
            .Returns(new Func<object, HttpResponseMessage>(r => new HttpResponseMessage(HttpStatusCode.OK)));
        cnbCsvReaderMock.Setup(s => s.GetRecords(It.IsAny<Stream>()))
            .Returns(() => new[] { new CnbExchangeRateModel() { Code = new Currency("USD"), Rate = 5, Amount = 2 } });

        var provider = new CnbExchangeRateProvider(loggerMock.Object, httpHandlerMock.Object, dateTimeProviderMock.Object, cnbCsvReaderMock.Object);
        provider.SetEndpoints(new[] { endpoint });
        provider.FetchRates();
        var exchangeRates = provider.GetExchangeRates(new[] { currencyToSearch }).ToList();

        Assert.AreEqual(0, exchangeRates.Count);
    }
}