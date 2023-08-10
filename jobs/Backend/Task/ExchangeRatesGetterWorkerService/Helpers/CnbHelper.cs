using Newtonsoft.Json;

namespace ExchangeRatesGetterWorkerService.Helpers
{
    public class Rootobject
    {
        public Rate[] rates { get; set; }
    }

    public class Rate
    {
        public string validFor { get; set; }
        public int order { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string currencyCode { get; set; }
        public float rate { get; set; }
    }

    public class CnbHelper
    {
        private static readonly HttpClient client = new HttpClient();

        private ILogger _logger;


        public CnbHelper(ILogger logger)
        {
            _logger = logger;
        }

 
        public async Task<Rate[]> GetMainCurrenciesValidRates()
        {
            try
            {
                DateTime date = DateTimeHelper.GetMainCurrenciesLastPublicationDate();

                using HttpResponseMessage response = await client.GetAsync($"https://api.cnb.cz/cnbapi/exrates/daily?date={date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}&lang=EN");
                _logger.LogInformation(
                    "Requesting url: " + $"https://api.cnb.cz/cnbapi/exrates/daily?date={date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}&lang=EN"
                    );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();


                return JsonConvert.DeserializeObject<Rootobject>(responseBody).rates;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("\nException Caught in 'GetMainCurrenciesValidData' method!");
                _logger.LogInformation("Message :{0} ", e.Message);
            }

            return null;

        }

        public async Task<Rate[]> GetOtherCurrenciesValidRates()
        {

            try
            {
                DateTime date = DateTimeHelper.GetCestTimeFromUtcTime(DateTime.UtcNow).AddMonths(-1);

                using HttpResponseMessage response = 
                    await client.GetAsync($"https://api.cnb.cz/cnbapi/fxrates/daily-month?lang=EN&yearMonth={date.Year}-{date.Month.ToString("00")}");
                _logger.LogInformation(
                    "Requesting url: " + $"https://api.cnb.cz/cnbapi/fxrates/daily-month?lang=EN&yearMonth={date.Year}-{date.Month.ToString("00")}"
                    );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();


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
