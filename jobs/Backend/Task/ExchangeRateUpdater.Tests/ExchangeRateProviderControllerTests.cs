using System.Net.Mime;
using System.Text;
using ExchangeRateUpdater.WebApi.Models;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.WebApi.Tests;

[TestClass]
public class ExchangeRateProviderControllerTests : IntegrationTest
{
    private readonly IEnumerable<Currency> _currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    private readonly IEnumerable<Currency> _emptyCurrencies = new Currency[]
    {
    };


    [TestMethod]
    public async Task ExchangeRates_ReturnsRates()
    {
        //arrange
        var stringContent = new StringContent(JsonConvert.SerializeObject(_currencies), Encoding.UTF8,
            MediaTypeNames.Application.Json);

        //act
        var responseMessage = await TestClient?.PostAsync(Routes.ExchangeRates, stringContent)!;
        responseMessage.EnsureSuccessStatusCode();

        //assert
        ServiceResponse<IEnumerable<ExchangeRate>> exchangeRates = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<ExchangeRate>>>(await responseMessage.Content.ReadAsStringAsync());
        Assert.IsTrue(exchangeRates.Success);
        Assert.IsTrue(exchangeRates.Data?.Any());
    }

    [TestMethod]
    public async Task ExchangeRates_EmptyCurrencies_ReturnsEmptyRates()
    {
        //arrange
        var stringContent = new StringContent(JsonConvert.SerializeObject(_emptyCurrencies), Encoding.UTF8,
            MediaTypeNames.Application.Json);

        //act
        var responseMessage = await TestClient?.PostAsync(Routes.ExchangeRates, stringContent)!;
        responseMessage.EnsureSuccessStatusCode();

        //assert
        ServiceResponse<IEnumerable<ExchangeRate>> exchangeRates = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<ExchangeRate>>>(await responseMessage.Content.ReadAsStringAsync());
        Assert.IsTrue(exchangeRates.Success);
        Assert.IsFalse(exchangeRates.Data?.Any());
    }
}