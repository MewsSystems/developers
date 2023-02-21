using ExchangeRateUpdater.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Abstractions
{
    public interface IExchangeRateSourceOld
    {
        Task LoadAsync();
        IEnumerable<ExchangeRate> GetSourceExchangeRates(Currency currency);
        IEnumerable<ExchangeRate> GetTargetExchangeRates(Currency currency);
    }
}