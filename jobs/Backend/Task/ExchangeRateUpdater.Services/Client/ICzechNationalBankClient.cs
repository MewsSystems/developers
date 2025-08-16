using ExchangeRateUpdater.Services.Client.ClientModel;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Client
{
    public interface ICzechNationalBankClient
    {
        //https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates/dailyUsingGET_1
        [Get("/cnbapi/exrates/daily")]
        public Task<ExchangeRateResponseList> GetDailyRatesAsync(string lang);
    }
}
