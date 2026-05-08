using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.DTOs;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Domain.Validators;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Mappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.Infrastructure;

[TestFixture]
public class CzechNationalBankExchangeRateProviderTests
{
    private Mock<HttpMessageHandler> _httpHandlerMock;
    private HttpClient _httpClient;
    private IOptions<ExchangeRateProviderOptions> _options;
    private IValidator<CnbRateDto> _rateDtoValidator ;
    private IExchangeRateMapper _mapper;
    private CzechNationalBankExchangeRateProvider _provider;

    [SetUp]
    public void SetUp()
    {
        _httpHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpHandlerMock.Object);
        
        _options = Options.Create(new ExchangeRateProviderOptions
        {
            BaseUrl = "https://test.cnb.cz/cnbapi/exrates/daily"
        });

        _rateDtoValidator  = new CnbRateDtoValidator();
        _mapper = new CnbExchangeRateMapper(
            Options.Create(new CurrencyOptions { BaseCurrency = "CZK", Currencies = new[] { "USD", "EUR" } }),
            new Mock<ILogger<CnbExchangeRateMapper>>().Object);

        _provider = new CzechNationalBankExchangeRateProvider(_httpClient, _rateDtoValidator , _mapper, _options);
    }

    [Test]
    public async Task GetExchangeRates_WhenValidResponse_ReturnsExpectedRates()
    {
        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var cnbRateDto = new CnbRateDto
        {
            ExchangeRateDtos = new List<CnbExchangeRateDto>
            {
                new() { CurrencyCode = "USD", Amount = 1, Rate = 22.114m, ValidFor = today },
                new() { CurrencyCode = "EUR", Amount = 1, Rate = 25.070m, ValidFor = today }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(cnbRateDto);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var currencies = new[] { new Currency("USD"), new Currency("EUR"), new Currency("CZK") };
        
        var result = await _provider.GetExchangeRates(currencies);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetExchangeRates_WhenInvalidResponse_ThrowsException()
    {
        var invalidJson = "{\"invalid\":\"data\"}";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(invalidJson)
        };

        _httpHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var currencies = new[] { new Currency("USD") };
        
        Assert.ThrowsAsync<Exception>(async () =>
            await _provider.GetExchangeRates(currencies));
    }
}