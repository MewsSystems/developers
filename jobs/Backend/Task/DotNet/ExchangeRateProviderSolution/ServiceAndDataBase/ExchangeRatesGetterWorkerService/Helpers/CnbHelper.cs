
using Newtonsoft.Json;

namespace ExchangeRatesGetterWorkerService.Helpers
{
    /// <summary>   A rootobject. </summary>
    ///
    public class Rootobject
    {
        /// <summary>   Gets or sets the rates. </summary>
        ///
        /// <value> The rates. </value>
        public Rate[] rates { get; set; }
    }

    /// <summary>   A rate. </summary>
    ///
    /// <remarks>   , 13.08.2023. </remarks>

    public class Rate
    {
        /// <summary>   Gets or sets the valid for. </summary>
        ///
        /// <value> The valid for. </value>
        public string validFor { get; set; }

        /// <summary>   Gets or sets the order. </summary>
        ///
        /// <value> The order. </value>
        public int order { get; set; }

        /// <summary>   Gets or sets the country. </summary>
        ///
        /// <value> The country. </value>
        public string country { get; set; }

        /// <summary>   Gets or sets the currency. </summary>
        ///
        /// <value> The currency. </value>
        public string currency { get; set; }

        /// <summary>   Gets or sets the amount. </summary>
        ///
        /// <value> The amount. </value>
        public int amount { get; set; }

        /// <summary>   Gets or sets the currency code. </summary>
        ///
        /// <value> The currency code. </value>
        public string currencyCode { get; set; }

        /// <summary>   Gets or sets the rate. </summary>
        ///
        /// <value> The rate. </value>
        public float rate { get; set; }
    }

    /// <summary>   A cnb helper. </summary>
    ///
    public class CnbHelper
    {
        /// <summary>   (Immutable) the client. </summary>
        private static readonly HttpClient client = new HttpClient();

        /// <summary>   The logger. </summary>
        private ILogger _logger;

        /// <summary>   Constructor. </summary>
        ///
        ///
        /// <param name="logger">   The logger. </param>
        public CnbHelper(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>   Gets main currencies valid rates. </summary>
        ///
        ///
        /// <returns>   The main currencies valid rates. </returns>
        public async Task<Rate[]> GetMainCurrenciesValidRates()
        {
            try
            {
                _logger.LogInformation("Request Main rates from CNB at: {time}", DateTimeOffset.Now);
                DateTime date = DateTimeHelper.GetMainCurrenciesLastPublicationDate();

                using HttpResponseMessage response = await client.GetAsync($"https://api.cnb.cz/cnbapi/exrates/daily?date={date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}&lang=EN");
                _logger.LogInformation(
                    "Requesting url: " + $"https://api.cnb.cz/cnbapi/exrates/daily?date={date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}&lang=EN"
                    );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Request Main rates from CNB: Success.");

                return JsonConvert.DeserializeObject<Rootobject>(responseBody).rates;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("\nException Caught in 'GetMainCurrenciesValidData' method!");
                _logger.LogInformation("Message :{0} ", e.Message);
            }

            return null;

        }

        /// <summary>   Gets other currencies valid rates. </summary>
        ///
        ///
        /// <returns>   The other currencies valid rates. </returns>
        public async Task<Rate[]> GetOtherCurrenciesValidRates()
        {

            try
            {
                _logger.LogInformation("Request Other rates from CNB at: {time}", DateTimeOffset.Now);
                DateTime date = DateTimeHelper.GetCestTimeFromUtcTime(DateTime.UtcNow).AddMonths(-1);

                using HttpResponseMessage response = 
                    await client.GetAsync($"https://api.cnb.cz/cnbapi/fxrates/daily-month?lang=EN&yearMonth={date.Year}-{date.Month.ToString("00")}");
                _logger.LogInformation(
                    "Requesting url: " + $"https://api.cnb.cz/cnbapi/fxrates/daily-month?lang=EN&yearMonth={date.Year}-{date.Month.ToString("00")}"
                    );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Request Other rates from CNB: Success.");

                return JsonConvert.DeserializeObject<Rootobject>(responseBody).rates;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("\nException Caught in 'GetMainCurrenciesValidData' method!");
                _logger.LogInformation("Message :{0} ", e.Message);
            }

            return null;
        }


    }
}
