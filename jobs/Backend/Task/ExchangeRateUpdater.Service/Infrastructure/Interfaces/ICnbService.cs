using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Service.Infrastructure.Interfaces
{
    public interface ICnbService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesByCurrencyAsync(DateTime date, IEnumerable<Currency> currencies);
    }
}
