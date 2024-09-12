using ExchangeRateUpdater.Exchange_Providers.Provider.CNB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExchangeRateUpdater.Tests.CNB
{
    public class CNBResponse_Tests
    {
        /// <summary>
        /// Tests the deserialization of a CNB response JSON string into a CNB_Response object.
        /// </summary>
        [Fact]
        public void CNB_Response_Deserialization()
        {
            // Arrange
            string json = "{\"rates\":[{\"validFor\":\"2023-09-26\",\"order\":186,\"country\":\"USA\",\"currency\":\"dolar\",\"amount\":1,\"currencyCode\":\"USD\",\"rate\":25.0}]}";

            // Act
            CNB_Response response = JsonConvert.DeserializeObject<CNB_Response>(json, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Rates);
            Assert.Single(response.Rates);

            CNB_Exchange_Rate exchangeRate = response.Rates.First();
            Assert.NotNull(exchangeRate);
            Assert.Equal(new DateTime(2023, 9, 26), exchangeRate.ValidFor);
            Assert.Equal(186, exchangeRate.Order);
            Assert.Equal("USA", exchangeRate.Country);
            Assert.Equal("dolar", exchangeRate.Currency);
            Assert.Equal(1, exchangeRate.Amount);
            Assert.Equal("USD", exchangeRate.CurrencyCode);
            Assert.Equal(25.0, exchangeRate.Rate);
        }
    }
}
