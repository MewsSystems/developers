using System.Net;
using ExchangeRate.Api.Clients;
using ExchangeRate.Domain.Extensions;
using ExchangeRate.Domain.Providers.CzechNationalBank;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace ExchangeRate.UnitTests;

[TestFixture]
public class CzechNationalBankClientTests
{
    [SetUp]
    public void SetUp()
    {
        _mockHttp = new MockHttpMessageHandler();
        _httpClient = _mockHttp.ToHttpClient();
        _czechNationalBankClient = new CzechNationalBankClient(_httpClient);
        _baseAddressUri = new Uri("https://api.cnb.cz/cnbapi/exrates/daily");
    }

    private MockHttpMessageHandler _mockHttp;
    private HttpClient _httpClient;
    private CzechNationalBankClient _czechNationalBankClient;
    private Uri _baseAddressUri;

    [Test]
    [TestCase(null, null)]
    [TestCase("2024-01-01", null)]
    [TestCase(null, "CZ")]
    [TestCase("2024-01-01", "CZ")]
    public async Task GetExchangeRatesAsync_SuccessfulResponse_ReturnsOkResultWithProviderResponse(
        string date,
        string lang)
    {
        // Arrange
        var exchangeRate =
            new CzechNationalBankExchangeRate(1, "Australia", 1, DateTime.Today, "dollar", "AUD", 15.0M);
        var expectedResponse = new CzechNationalBankProviderResponse([exchangeRate]);
        var expectedResponseJson = JsonConvert.SerializeObject(expectedResponse);
        var request = new CzechNationalBankProviderRequest(date, lang);
        var requestUri = request.ConstructRequestUri(_baseAddressUri);

        _mockHttp
            .When(requestUri.ToString())
            .Respond("application/json", expectedResponseJson);

        // Act
        var result =
            await _czechNationalBankClient.GetExchangeRatesAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        var providerResponse = ((Ok<CzechNationalBankProviderResponse>)result).Value;
        providerResponse.Should().NotBeNull();

        var providerResponseRates = providerResponse.Rates.ToArray();
        providerResponseRates.Length.Should().Be(1);

        var providerResponseRate = providerResponseRates.First();
        providerResponseRate.Should().BeEquivalentTo(exchangeRate);
    }

    [Test]
    [TestCase(null, null)]
    [TestCase("0000-00-00", null)]
    [TestCase(null, "XX")]
    [TestCase("0000-00-01", "XX")]
    public async Task GetExchangeRatesAsync_FailedResponse_ReturnsProblemResult(
        string date,
        string lang)
    {
        // Arrange
        var statusCode = HttpStatusCode.BadRequest;
        var request = new CzechNationalBankProviderRequest(date, lang);
        var requestUri = request.ConstructRequestUri(_baseAddressUri);

        _mockHttp
            .When(requestUri.ToString())
            .Respond(statusCode);

        // Act
        var result =
            await _czechNationalBankClient.GetExchangeRatesAsync(request, CancellationToken.None);

        // Assert
        var problemResult = (ProblemHttpResult)result;

        problemResult.StatusCode.Should().Be((int)statusCode);
    }

    [Test]
    public void Name_ReturnsExpectedName()
    {
        _czechNationalBankClient.Name.Should().Be("Czech National Bank");
    }

    [Test]
    public void BaseAddress_ReturnsExpectedBaseAddress()
    {
        _czechNationalBankClient.BaseAddress.Should().Be(_baseAddressUri);
    }
}