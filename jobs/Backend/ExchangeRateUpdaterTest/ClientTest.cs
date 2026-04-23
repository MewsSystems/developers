using ExchangeRateUpdater.ExchangeRateAPI.CBNClientApi;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdaterTest
{
    public class ClientTest
    {
        [Fact]
        public void GivenAnAPI_WhenGetExratesDailyIsCalled_ResultIsMappedCorrectly()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CBNClientApi>>();

            var json = """{"rates":[{"validFor":"2024-05-10","order":90,"country":"Austrálie","currency":"dolar","amount":1,"currencyCode":"AUD","rate":15.285},{"validFor":"2024-05-10","order":90,"country":"Brazílie","currency":"real","amount":1,"currencyCode":"BRL","rate":4.503},{"validFor":"2024-05-10","order":90,"country":"Bulharsko","currency":"lev","amount":1,"currencyCode":"BGN","rate":12.750},{"validFor":"2024-05-10","order":90,"country":"Čína","currency":"žen-min-pi","amount":1,"currencyCode":"CNY","rate":3.202},{"validFor":"2024-05-10","order":90,"country":"Dánsko","currency":"koruna","amount":1,"currencyCode":"DKK","rate":3.342},{"validFor":"2024-05-10","order":90,"country":"EMU","currency":"euro","amount":1,"currencyCode":"EUR","rate":24.935},{"validFor":"2024-05-10","order":90,"country":"Filipíny","currency":"peso","amount":100,"currencyCode":"PHP","rate":40.255},{"validFor":"2024-05-10","order":90,"country":"Hongkong","currency":"dolar","amount":1,"currencyCode":"HKD","rate":2.960},{"validFor":"2024-05-10","order":90,"country":"Indie","currency":"rupie","amount":100,"currencyCode":"INR","rate":27.698},{"validFor":"2024-05-10","order":90,"country":"Indonesie","currency":"rupie","amount":1000,"currencyCode":"IDR","rate":1.442},{"validFor":"2024-05-10","order":90,"country":"Island","currency":"koruna","amount":100,"currencyCode":"ISK","rate":16.590},{"validFor":"2024-05-10","order":90,"country":"Izrael","currency":"nový šekel","amount":1,"currencyCode":"ILS","rate":6.219},{"validFor":"2024-05-10","order":90,"country":"Japonsko","currency":"jen","amount":100,"currencyCode":"JPY","rate":14.852},{"validFor":"2024-05-10","order":90,"country":"Jižní Afrika","currency":"rand","amount":1,"currencyCode":"ZAR","rate":1.255},{"validFor":"2024-05-10","order":90,"country":"Kanada","currency":"dolar","amount":1,"currencyCode":"CAD","rate":16.910},{"validFor":"2024-05-10","order":90,"country":"Korejská republika","currency":"won","amount":100,"currencyCode":"KRW","rate":1.693},{"validFor":"2024-05-10","order":90,"country":"Maďarsko","currency":"forint","amount":100,"currencyCode":"HUF","rate":6.431},{"validFor":"2024-05-10","order":90,"country":"Malajsie","currency":"ringgit","amount":1,"currencyCode":"MYR","rate":4.881},{"validFor":"2024-05-10","order":90,"country":"Mexiko","currency":"peso","amount":1,"currencyCode":"MXN","rate":1.380},{"validFor":"2024-05-10","order":90,"country":"MMF","currency":"ZPČ","amount":1,"currencyCode":"XDR","rate":30.498},{"validFor":"2024-05-10","order":90,"country":"Norsko","currency":"koruna","amount":1,"currencyCode":"NOK","rate":2.136},{"validFor":"2024-05-10","order":90,"country":"Nový Zéland","currency":"dolar","amount":1,"currencyCode":"NZD","rate":13.915},{"validFor":"2024-05-10","order":90,"country":"Polsko","currency":"zlotý","amount":1,"currencyCode":"PLN","rate":5.801},{"validFor":"2024-05-10","order":90,"country":"Rumunsko","currency":"leu","amount":1,"currencyCode":"RON","rate":5.011},{"validFor":"2024-05-10","order":90,"country":"Singapur","currency":"dolar","amount":1,"currencyCode":"SGD","rate":17.088},{"validFor":"2024-05-10","order":90,"country":"Švédsko","currency":"koruna","amount":1,"currencyCode":"SEK","rate":2.134},{"validFor":"2024-05-10","order":90,"country":"Švýcarsko","currency":"frank","amount":1,"currencyCode":"CHF","rate":25.503},{"validFor":"2024-05-10","order":90,"country":"Thajsko","currency":"baht","amount":100,"currencyCode":"THB","rate":62.950},{"validFor":"2024-05-10","order":90,"country":"Turecko","currency":"lira","amount":100,"currencyCode":"TRY","rate":71.779},{"validFor":"2024-05-10","order":90,"country":"USA","currency":"dolar","amount":1,"currencyCode":"USD","rate":23.131},{"validFor":"2024-05-10","order":90,"country":"Velká Británie","currency":"libra","amount":1,"currencyCode":"GBP","rate":28.979}]}""";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN")
                    .Respond("application/json", json);

            var httpClient = new HttpClient(mockHttp);
            httpClient.BaseAddress = new Uri("https://api.cnb.cz");

            // Act
            var client = new CBNClientApi(httpClient, mockLogger.Object);

            var result = client.GetExratesDaily();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(31, result.Result.Rates.Count());
        }

        [Fact]
        public async void GivenAnAPI_WhenGetExratesDailyIsCalled_ErrorIsHandled()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CBNClientApi>>();

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN")
                .Respond(req => new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotImplemented });

            var httpClient = new HttpClient(mockHttp);
            httpClient.BaseAddress = new Uri("https://api.cnb.cz");

            // Act
            var client = new CBNClientApi(httpClient, mockLogger.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => client.GetExratesDaily());

            // Assert
            Assert.Contains("Request did not return success status code.", exception.Message);
        }
    }
}
