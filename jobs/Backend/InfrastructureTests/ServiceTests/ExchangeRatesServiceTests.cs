using ApiClients.CzechNationalBank.Models;
using Core.Services.CzechNationalBank;
using Data;
using Data.Models;
using Data.Repositories.Interfaces;
using Infrastructure.CzechNationalBank.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Tests.ServiceTests;

public class ExchangeRatesServiceTests : ProductionTestFixture
{
    protected ExchangeRateService _exchangeRateService;

    [SetUp]
    public void Setup()
    {
        var options = _serviceProvider.GetService<IOptions<CzechNationalBankApiOptions>>();
        var repository = _serviceProvider.GetService<IGenericRepository<CurrencyCzechRate>>();
        _exchangeRateService = new ExchangeRateService(_czechNationalBankHttpApiClientMock.Object, options, repository, _checkBankRateServiceMock.Object, 
            new Log<ExchangeRateService>(_logger));
    }

    [Test]
    public async Task GetExchangeRate_RatesAccessToService_ReturnsCorrectRate()
    {
       var rates = "27Oct2023#208\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.851\nBrazil|real|1|BRL|4.705\nBulgaria|lev|1|BGN|12.613\n" +
            "Canada|dollar|1|CAD|16.934\nChina|renminbi|1|CNY|3.199\nDenmark|krone|1|DKK|3.305\nEMU|euro|1|EUR|24.670\nIndia|rupee|100|INR|28.119\n" +
            "Mexico|peso|1|MXN|1.294\nSweden|krona|1|SEK|2.086\nSwitzerland|franc|1|CHF|25.963\nUnited Kingdom|pound|1|GBP|28.358\nUSA|dollar|1|USD|23.408\n";
        
        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>()))
            .Returns(Task.FromResult(rates));

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("EUR"), new Currency("CAD") };        

        var result = await _exchangeRateService.GetExchangeRates(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].SourceCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(result[0].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result[0].Value, Is.EqualTo(24.670m));
            Assert.That(result[1].SourceCurrency.Code, Is.EqualTo("CAD"));
            Assert.That(result[1].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result[1].Value, Is.EqualTo(16.934m));
        });
    }

    [Test]
    public async Task GetExchangeRate_RatesNoAccessToService_ReturnsCorrectRate()
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

        var result = await _exchangeRateService.GetExchangeRates(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].SourceCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(result[0].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result[0].Value, Is.EqualTo(24.670m));
            Assert.That(result[1].SourceCurrency.Code, Is.EqualTo("CAD"));
            Assert.That(result[1].TargetCurrency.Code, Is.EqualTo("ZCH"));
            Assert.That(result[1].Value, Is.EqualTo(16.934m));
        });
    }

    [Test]
    public async Task GetExchangeRate_NoExistCurrencyRatesAccessToService_ReturnsCorrectEmptyRate()
    {
        var rates = "27Oct2023#208\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.851\nBrazil|real|1|BRL|4.705\nBulgaria|lev|1|BGN|12.613\n" +
            "Canada|dollar|1|CAD|16.934\nChina|renminbi|1|CNY|3.199\nDenmark|krone|1|DKK|3.305\nEMU|euro|1|EUR|24.670\nIndia|rupee|100|INR|28.119\n" +
            "Mexico|peso|1|MXN|1.294\nSweden|krona|1|SEK|2.086\nSwitzerland|franc|1|CHF|25.963\nUnited Kingdom|pound|1|GBP|28.358\nUSA|dollar|1|USD|23.408\n";

        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>()))
            .Returns(Task.FromResult(rates));

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("NOK"), new Currency("PHP") };

        var result = await _exchangeRateService.GetExchangeRates(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task GetExchangeRate_CurrencyRatesAccessToServThrowException_ReturnsInternalServerError()
    {
        _czechNationalBankHttpApiClientMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetExchangeRatesRequest>())).Throws(new Exception());

        _checkBankRateServiceMock.Setup(mock => mock.IsNeededCallCzechNationalBankRates()).Returns(true);

        var request = new List<Currency>() { new Currency("NOK"), new Currency("PHP") };

        Assert.ThrowsAsync<Exception>(async () => await _exchangeRateService.GetExchangeRates(request));
    }
}
