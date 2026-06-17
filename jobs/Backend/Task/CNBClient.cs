namespace ExchangeRateUpdater
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// HTTP Client for Czech National Bank API
    /// Link to Swagger: https://api.cnb.cz/cnbapi/swagger-ui.html#/
    /// </summary>
    public class CNBClient
    {
        private const string CZECH_CROWN = "CZK";

        private const string BASE_URL = "https://api.cnb.cz/cnbapi/";

        public Currency DefaultCurrency => new(CZECH_CROWN);

        /// <summary>
        /// Call Get latest Exchange Rates for Czech crown.
        /// The amount is normalized to 1 in all currencies.
        /// Meaning is the comparison to 1 Czech crown
        /// </summary>
        /// <returns>A Dictionary of Currency Code - Value exchange rate </returns>
        public async Task<IDictionary<string, decimal>> GetCurrentExchangeRates()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new System.Uri(BASE_URL);
            var httpResponse = await httpClient.GetAsync("exrates/daily?lang=EN");

            if (!httpResponse.IsSuccessStatusCode)
                throw new System.Exception("Could not retreive rates from CNB Client");

            var response =
                await httpResponse.Content.ReadFromJsonAsync<CNBGetExchangeRatesResponse>();

            return response.Rates.ToDictionary(
                keySelector: x => x.CurrencyCode,
                elementSelector: x => x.Rate / x.amount
            );
        }
    }
}
