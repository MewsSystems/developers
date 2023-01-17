using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankService
    {
        //https://publicapi.dev/czech-national-bank-api
        //Date parameter can be added in this format
        //?date={0:dd\.MM\.yyyy}
        private const string CNB_API_URL = @"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        private readonly HttpClient _client;

        protected CzechNationalBankService(HttpClient client) 
            => _client = client;

        public IEnumerable<CzechNationalBankExchangeRate> GetRates()
            => Get<IEnumerable<CzechNationalBankExchangeRate>>(CNB_API_URL)
                .Result;
        
        //Possible refactor if we had different bank clients
        private async Task<TResp> Get<TResp>(string urlPath)
        {
            var response = await _client.GetAsync(urlPath);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResp>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}