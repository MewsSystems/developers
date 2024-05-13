using ExchangeRate.Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Datalayer.Services
{
    public interface ICurrencyExchangeProvider
    {
        Task<IEnumerable<CurrencyExchangeRate>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies);
    }
}
