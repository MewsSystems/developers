using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank
{
    public interface ICzechNationalBankService
    {
        Task<string?> GetExchangeRatesByDay(DateTime date);
        Task<string?> GetDailyExchangeRates();
    }
}
