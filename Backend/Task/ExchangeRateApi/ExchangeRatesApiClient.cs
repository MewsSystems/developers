using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateApi
{
    /// <summary>
    /// Client for obtaining Rates from exchangeratesapi.io
    /// </summary>
    public class ExchangeRatesApiClient
    {
        private readonly string _url = @"https://api.exchangeratesapi.io/latest?base={0}&symbols={1}";

        /// <summary>
        /// Obtains exchange rate, for invalid parameters returns null
        /// </summary>
        /// <param name="baseCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <returns></returns>
        public async Task<ExchangeRate> GetExchangeRate(Currency baseCurrency, Currency targetCurrency)
        {
            var url = string.Format(_url, baseCurrency.Code, targetCurrency.Code);
            using (var client = new HttpClient())
            {
                try
                {
                    var jsonStr = await client.GetStringAsync(url);
                    var data = JsonConvert.DeserializeObject<ExchangeRatesResponse>(jsonStr);
                    if (data?.Rates.Count == 1)
                    {
                        var value = data.Rates[targetCurrency.Code];
                        return new ExchangeRate(baseCurrency, targetCurrency, value);
                    }
                }
                catch (HttpRequestException e)
                {
                    //log error somewhere
                }
                return null;
            }
        }
    }
}