using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank
{
    public interface IExchangeRatesService
    {
        string Rates(int rates);
        Task<List<ExchangeRateBank>> GetExchangeRatesByDay(DateTime date);
    }
}
