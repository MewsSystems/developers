using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public interface ICzechNationalBankApiClient
    {
        string TargetCurrencyCode { get; }

        Task<IReadOnlyList<BankApiExchangeRate>> GetDailyExchangeRatesAsync();        
    }
}
