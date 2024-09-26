using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Proxies
{
    public class CzechNationalBankProxy(ICzechNationalBankClient czechNationalBankClient) : IExchangeRateProxy
    {
        public async Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(DateTimeOffset? date)
        {
            ExRateDailyResponse response = await czechNationalBankClient.CnbapiExratesDailyAsync(date, Lang.EN);
            return response.Rates.Select(x => new CurrencyRate() { CurrencyCode = x.CurrencyCode, Rate = (decimal)x.Rate });
        }
    }
}
