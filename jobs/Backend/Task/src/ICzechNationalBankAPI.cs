using ExchangeRateUpdaterModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.src
{
    public interface ICzechNationalBankAPI
    {
        Task<IEnumerable<ExchangeRateModel>> GetRatesAsync();
    }
}
