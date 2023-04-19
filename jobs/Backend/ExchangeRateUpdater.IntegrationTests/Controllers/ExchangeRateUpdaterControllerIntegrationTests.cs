using ExchangeRateUpdater.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ExchangeRateUpdater.IntegrationTests.Controllers
{
    public class ExchangeRateUpdaterControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        public const string GetExchangeRatePostUrl = "ExchangeRateUpdater/GetExchangeRates";
        private IEnumerable<Currency> _inputCurrencies;
        private IEnumerable<ExchangeRate> _expectedExchangeRates;
        private readonly WebApplicationFactory<Program> _webApplicationfactory;

        public ExchangeRateUpdaterControllerIntegrationTests()
        {
            _webApplicationfactory = new WebApplicationFactory<Program>();
            _inputCurrencies = new[] 
            {
                new Currency("AUD"),
                new Currency("CAD"),
                new Currency("USD")
            };

            _expectedExchangeRates = new[]
            {
                new ExchangeRate(new Currency("CZK"), new Currency("AUD"), Convert.ToDecimal(14.364)),
                new ExchangeRate(new Currency("CZK"), new Currency("CAD"), Convert.ToDecimal(15.929)),
                new ExchangeRate(new Currency("CZK"), new Currency("USD"), Convert.ToDecimal(21.312))
            };
        }

        [Fact]
        public async Task ExchangeRateUpdaterController_PostMethod_ShouldRetrieveExchangeRates_WhenCorrectInputCurrenciesAreProvided()
        {
            var httpClient = _webApplicationfactory.CreateClient();
            var inputCurrenciesJsonContent = new StringContent(JsonSerializer.Serialize(_inputCurrencies), Encoding.UTF8,
                                                  MediaTypeNames.Application.Json);

            var response = await httpClient.PostAsync(GetExchangeRatePostUrl, inputCurrenciesJsonContent);
            var exchangeRates = JsonSerializer.Deserialize<IEnumerable<ExchangeRate>>(
                                    response.Content.ReadAsStringAsync().Result, 
                                    new JsonSerializerOptions
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });

            response.EnsureSuccessStatusCode();
            Assert.NotNull(exchangeRates);
            Assert.NotEmpty(exchangeRates);
            // At this point we cannot assert any specific exchange rates has been retrieved as the rates may change
            // but, for the purpose of this exercise, I will include a commented out assertion as if we had other source
            // of truth for the exchange rates. This can cause this test to fail if the CNB changes the rates.
            //Assert.Equal(_expectedExchangeRates, exchangeRates);
        }
    }
}