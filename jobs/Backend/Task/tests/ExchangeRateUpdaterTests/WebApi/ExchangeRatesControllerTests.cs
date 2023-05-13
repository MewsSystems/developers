using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdaterTests.WebApi.Fixtures;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdaterTests.WebApi
{
    public class ExchangeRatesControllerTests : IClassFixture<ExchangeRateUpdaterWebApplicationFactory>
    {
        private readonly HttpClient _exchangeRateUpdaterClient;
        
        public ExchangeRatesControllerTests(ExchangeRateUpdaterWebApplicationFactory webApplicationFactory)
        {
            _exchangeRateUpdaterClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task GetByCurrencies_When_CurrenciesIsNull_Then_Returns400()
        {
            // Act
            var respose = await _exchangeRateUpdaterClient.PostAsync("/exchangeRates", JsonContent.Create((IEnumerable<string>)null!));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, respose.StatusCode);    
        }

        [Fact]
        public async Task GetByCurrencies_When_CurrenciesIsEmpty_Then_Returns400()
        {
            // Act            
            var respose = await _exchangeRateUpdaterClient.PostAsync("/exchangeRates", JsonContent.Create(new List<string>()));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, respose.StatusCode);
        }

        [Fact]
        public async Task GetByCurrencies_When_CurrenciesAreNotAvailable_Then_Returns404()
        {
            // Act            
            var respose = await _exchangeRateUpdaterClient.PostAsync("/exchangeRates", JsonContent.Create(new List<string> { "IGS" }));

            // Assert            
            Assert.Equal(HttpStatusCode.NotFound, respose.StatusCode);
        }

        [Fact]
        public async Task GetByCurrencies_When_CurrenciesAreAvailable_Then_Returns200WithExchangeRates()
        {
            // Act            
            var respose = await _exchangeRateUpdaterClient.PostAsync("/exchangeRates", JsonContent.Create(new List<string> { "USD", "SEK", "IGS" }));

            // Assert            
            Assert.Equal(HttpStatusCode.OK, respose.StatusCode);

            var exchangeRates = await respose.Content.ReadFromJsonAsync<IEnumerable<ExchangeRate>>();

            Assert.NotNull(exchangeRates);
            Assert.NotEmpty(exchangeRates);
            Assert.Equal(2, exchangeRates.Count());
            Assert.Contains(exchangeRates!, er => er.TargetCurrency.Code == "USD");
            Assert.Contains(exchangeRates!, er => er.TargetCurrency.Code == "SEK");
        }
    }
}
