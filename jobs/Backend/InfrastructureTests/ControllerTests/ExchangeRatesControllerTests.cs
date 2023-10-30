using Data;
using ExchangeRateUpdate.Controllers;
using Infrastructure.CzechNationalBank.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Services.Handlers.CzechNationalBank.Response;

namespace Tests.ControllerTests;

public class ExchangeRatesControllerTests : ProductionTestFixture
{
    protected ExchangeRatesController _controller;

    [SetUp]
    public void Setup()
    {
        var mediator = _serviceProvider.GetService<IMediator>();
        _controller = new ExchangeRatesController(mediator, new Log<ExchangeRatesController>(_logger));
    }

    [Test]
    public async Task CurrencyRateLowerCase_RatesAccessToServiceWithEmptyDatabase_ReturnsCorrectRate()
    {
       var rates = "27Oct2023#208\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.851\nBrazil|real|1|BRL|4.705\nBulgaria|lev|1|BGN|12.613\n" +
            "Canada|dollar|1|CAD|16.934\nChina|renminbi|1|CNY|3.199\nDenmark|krone|1|DKK|3.305\nEMU|euro|1|EUR|24.670\nIndia|rupee|100|INR|28.119\n" +
            "Mexico|peso|1|MXN|1.294\nSweden|krona|1|SEK|2.086\nSwitzerland|franc|1|CHF|25.963\nUnited Kingdom|pound|1|GBP|28.358\nUSA|dollar|1|USD|23.408\n";
        
        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>()))
            .Returns(Task.FromResult(rates));

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("eur"), new Currency("cad") };        

        var actionResult = (ContentResult)await _controller.GetExchangeRates(request);

        var result = JsonConvert.DeserializeObject<GetRateResponse>(actionResult.Content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExchangeRates, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result.ExchangeRates[0].SourceCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(result.ExchangeRates[0].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[0].Value, Is.EqualTo(24.670m));
            Assert.That(result.ExchangeRates[1].SourceCurrency.Code, Is.EqualTo("CAD"));
            Assert.That(result.ExchangeRates[1].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[1].Value, Is.EqualTo(16.934m));
        });
    }

    [Test]
    public async Task CurrencyRate_RatesNoAccessToServiceWithDatabaseData_ReturnsCorrectRate()
    {
        _currencyCzechRateBuilder
        .WithCode("EUR")
        .WithAmount(1)
        .WithCurrency("euro")
        .WithCountry("Europa")
        .WithRate(24.670m)
        .WithCreatedDate(DateTime.Now.AddDays(-2))
        .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.934m)
       .WithCreatedDate(DateTime.Now.AddDays(-2))
       .Create();

        _currencyCzechRateBuilder
        .WithCode("EUR")
        .WithAmount(1)
        .WithCurrency("euro")
        .WithCountry("Europa")
        .WithRate(24.670m)
        .WithCreatedDate(DateTime.Now.AddDays(-1))
        .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.934m)
       .WithCreatedDate(DateTime.Now.AddDays(-1))
       .Create();

        _currencyCzechRateBuilder
        .WithCode("EUR")
        .WithAmount(1)
        .WithCurrency("euro")
        .WithCountry("Europa")
        .WithRate(24.555m)
        .WithCreatedDate(DateTime.Now)
        .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.777m)
       .WithCreatedDate(DateTime.Now)
       .Create();

        var request = new List<Currency>() { new Currency("EUR"), new Currency("CAD") };

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(false);

        var actionResult = (ContentResult)await _controller.GetExchangeRates(request);

        var result = JsonConvert.DeserializeObject<GetRateResponse>(actionResult.Content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExchangeRates, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result.ExchangeRates[0].SourceCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(result.ExchangeRates[0].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[0].Value, Is.EqualTo(24.555m));
            Assert.That(result.ExchangeRates[1].SourceCurrency.Code, Is.EqualTo("CAD"));
            Assert.That(result.ExchangeRates[1].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[1].Value, Is.EqualTo(16.777m));
        });
    }

    [Test]
    public async Task CurrencyRate_RatesNoAccessToServiceWithOneSetDataInDatabase_ReturnsCorrectRate()
    {
        _currencyCzechRateBuilder
        .WithCode("EUR")
        .WithAmount(1)
        .WithCurrency("euro")
        .WithCountry("Europa")
        .WithRate(24.670m)
        .WithCreatedDate(DateTime.Now)
        .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.934m)
       .WithCreatedDate(DateTime.Now)
       .Create();

        var request = new List<Currency>() { new Currency("EUR"), new Currency("CAD") };

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(false);

        var actionResult = (ContentResult)await _controller.GetExchangeRates(request);

        var result = JsonConvert.DeserializeObject<GetRateResponse>(actionResult.Content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExchangeRates, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result.ExchangeRates[0].SourceCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(result.ExchangeRates[0].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[0].Value, Is.EqualTo(24.670m));
            Assert.That(result.ExchangeRates[1].SourceCurrency.Code, Is.EqualTo("CAD"));
            Assert.That(result.ExchangeRates[1].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[1].Value, Is.EqualTo(16.934m));
        });
    }

    [Test]
    public async Task CurrencyRate_RatesAccessToServiceWithDatabaseData_ReturnsCorrectRate()
    {
        _currencyCzechRateBuilder
       .WithCode("EUR")
       .WithAmount(1)
       .WithCurrency("euro")
       .WithCountry("Europa")
       .WithRate(24.666m)
       .WithCreatedDate(DateTime.Now.AddDays(-2))
       .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.888m)
       .WithCreatedDate(DateTime.Now.AddDays(-2))
       .Create();

        _currencyCzechRateBuilder
        .WithCode("EUR")
        .WithAmount(1)
        .WithCurrency("euro")
        .WithCountry("Europa")
        .WithRate(24.555m)
        .WithCreatedDate(DateTime.Now.AddDays(-1))
        .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.777m)
       .WithCreatedDate(DateTime.Now.AddDays(-1))
       .Create();
     
        var rates = "27Oct2023#208\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.851\nBrazil|real|1|BRL|4.705\nBulgaria|lev|1|BGN|12.613\n" +
            "Canada|dollar|1|CAD|16.934\nChina|renminbi|1|CNY|3.199\nDenmark|krone|1|DKK|3.305\nEMU|euro|1|EUR|24.670\nIndia|rupee|100|INR|28.119\n" +
            "Mexico|peso|1|MXN|1.294\nSweden|krona|1|SEK|2.086\nSwitzerland|franc|1|CHF|25.963\nUnited Kingdom|pound|1|GBP|28.358\nUSA|dollar|1|USD|23.408\n";

        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>()))
            .Returns(Task.FromResult(rates));

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("EUR"), new Currency("CAD") };

        var actionResult = (ContentResult)await _controller.GetExchangeRates(request);

        var result = JsonConvert.DeserializeObject<GetRateResponse>(actionResult.Content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExchangeRates, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result.ExchangeRates[0].SourceCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(result.ExchangeRates[0].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[0].Value, Is.EqualTo(24.670m));
            Assert.That(result.ExchangeRates[1].SourceCurrency.Code, Is.EqualTo("CAD"));
            Assert.That(result.ExchangeRates[1].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result.ExchangeRates[1].Value, Is.EqualTo(16.934m));
        });
    }

    [Test]
    public async Task CurrencyRate_RatesAccessToServiceWithDatabaseData_ReturnsCorrectEmptyRate()
    {
        _currencyCzechRateBuilder
       .WithCode("EUR")
       .WithAmount(1)
       .WithCurrency("euro")
       .WithCountry("Europa")
       .WithRate(24.670m)
       .WithCreatedDate(DateTime.Now.AddDays(-2))
       .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.934m)
       .WithCreatedDate(DateTime.Now.AddDays(-2))
       .Create();

        _currencyCzechRateBuilder
        .WithCode("EUR")
        .WithAmount(1)
        .WithCurrency("euro")
        .WithCountry("Europa")
        .WithRate(24.670m)
        .WithCreatedDate(DateTime.Now.AddDays(-1))
        .Create();

        _currencyCzechRateBuilder
       .WithCode("CAD")
       .WithAmount(1)
       .WithCurrency("dollar")
       .WithCountry("Canada")
       .WithRate(16.934m)
       .WithCreatedDate(DateTime.Now.AddDays(-1))
       .Create();

        var rates = "27Oct2023#208\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.851\nBrazil|real|1|BRL|4.705\nBulgaria|lev|1|BGN|12.613\n" +
            "Canada|dollar|1|CAD|16.934\nChina|renminbi|1|CNY|3.199\nDenmark|krone|1|DKK|3.305\nEMU|euro|1|EUR|24.670\nIndia|rupee|100|INR|28.119\n" +
            "Mexico|peso|1|MXN|1.294\nSweden|krona|1|SEK|2.086\nSwitzerland|franc|1|CHF|25.963\nUnited Kingdom|pound|1|GBP|28.358\nUSA|dollar|1|USD|23.408\n";

        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>()))
            .Returns(Task.FromResult(rates));

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("NOK"), new Currency("PHP") };

        var actionResult = (ContentResult)await _controller.GetExchangeRates(request);

        var result = JsonConvert.DeserializeObject<GetRateResponse>(actionResult.Content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExchangeRates, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task CurrencyRate_CurrencyRatesAccessToServThrowException_ReturnsInternalServerError()
    {
        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>())).Throws(new Exception());

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("NOK"), new Currency("PHP") };

        var actionResult = await _controller.GetExchangeRates(request);

        var result = actionResult as StatusCodeResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(500));
    }
}
