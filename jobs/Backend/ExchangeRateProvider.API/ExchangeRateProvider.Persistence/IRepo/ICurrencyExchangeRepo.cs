using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Persistence.IRepo
{
    public interface ICurrencyExchangeRepo
    {
        Task<string> GetPairsAsync(string uri);
    }
}